using ConsoleClan.Data.Entities;
using ConsoleClan.Data.Interfaces;
using ConsoleClan.Interfaces;
using ConsoleClan.Models;
using ConsoleClan.Utilities;

using CsvHelper;

using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

using Attack = ConsoleClan.Data.Entities.Attack;
using War = ConsoleClan.Data.Entities.War;

namespace ConsoleClan
{
	internal class Clash : IClash
	{
		private HttpClient httpClient = new HttpClient();
		private JsonSerializerOptions options;
		private string clanTag = "#PPY0VPRG";
		private string clanTagEncoded;
		private readonly IPlayerRepository playerRepository;
		private readonly ILeagueRepository leagueRepository;
		private readonly IWarRepository warRepository;
		private readonly ILeagueWarRepository leagueWarRepository;
		private readonly IAttackRepository attackRepository;
		private readonly IWarPlayerRepository warPlayerRepository;

		public Clash(
			IPlayerRepository memberRepository,
			ILeagueRepository leagueRepository,
			IWarRepository warRepository,
			ILeagueWarRepository leagueWarRepository,
			IAttackRepository attackRepository,
			IWarPlayerRepository warPlayerRepository)
		{
			options = new JsonSerializerOptions();
			options.Converters.Add(new DateTimeOffsetConverterUsingDateTimeParse());
			clanTagEncoded = WebUtility.UrlEncode(clanTag);
			this.playerRepository = memberRepository;
			this.leagueRepository = leagueRepository;
			this.warRepository = warRepository;
			this.leagueWarRepository = leagueWarRepository;
			this.attackRepository = attackRepository;
			this.warPlayerRepository = warPlayerRepository;
		}

		public async Task ProcessAsync(AuthenticationTokeReference authenticationTokeReference, string? fileName)
		{
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
				authenticationTokeReference == AuthenticationTokeReference.Office ?
				"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsImtpZCI6IjI4YTMxOGY3LTAwMDAtYTFlYi03ZmExLTJjNzQzM2M2Y2NhNSJ9.eyJpc3MiOiJzdXBlcmNlbGwiLCJhdWQiOiJzdXBlcmNlbGw6Z2FtZWFwaSIsImp0aSI6ImY4YzJjOTg0LTA1NjItNDA5Mi1iMTIyLTBjZTBhNjIzNGIzMyIsImlhdCI6MTY3MDk1MzU0Mywic3ViIjoiZGV2ZWxvcGVyLzRjZmEzY2FhLTQyY2EtNDg3YS00YzUxLTgzZGZkZWJiNWIzNyIsInNjb3BlcyI6WyJjbGFzaCJdLCJsaW1pdHMiOlt7InRpZXIiOiJkZXZlbG9wZXIvc2lsdmVyIiwidHlwZSI6InRocm90dGxpbmcifSx7ImNpZHJzIjpbIjE5NS4yMy4xOTguMjM5Il0sInR5cGUiOiJjbGllbnQifV19.jd1Yv3NerkMRXF6IuhRJY5RRFarlYXSdv3h31WQ6zHQSxgFUtXq5vRRAElgBzyHW9B76mRFX9OYZhe1by7TAeA" :
				"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiIsImtpZCI6IjI4YTMxOGY3LTAwMDAtYTFlYi03ZmExLTJjNzQzM2M2Y2NhNSJ9.eyJpc3MiOiJzdXBlcmNlbGwiLCJhdWQiOiJzdXBlcmNlbGw6Z2FtZWFwaSIsImp0aSI6ImI1Y2Q0OWRhLTgxNDAtNDdkNy05MjhiLWM5MmU1OThhZjhhNSIsImlhdCI6MTY2NzIyODIwNCwic3ViIjoiZGV2ZWxvcGVyLzRjZmEzY2FhLTQyY2EtNDg3YS00YzUxLTgzZGZkZWJiNWIzNyIsInNjb3BlcyI6WyJjbGFzaCJdLCJsaW1pdHMiOlt7InRpZXIiOiJkZXZlbG9wZXIvc2lsdmVyIiwidHlwZSI6InRocm90dGxpbmcifSx7ImNpZHJzIjpbIjk0LjYyLjcyLjMyIl0sInR5cGUiOiJjbGllbnQifV19.QKzT4hKQqmt_xQwaDVN6cVQVVHOz9yeOp7NLDKA6a9JCe-hbseMSFJatswg5NUixxLEoHwrsbpwp0py_KZ-R_A"
				);
			var clanDetail = await GetAndDeserializeAsync<ClanDetail>($"https://api.clashofclans.com/v1/clans/{clanTagEncoded}");
			if (clanDetail == null)
			{
				return;
			}
			var memberScores = new Dictionary<string, MemberScore>();
			var memberDetails = clanDetail.memberList.EmptyIfNull();
			await UpsertPlayerAsync(memberDetails);
			foreach (var memberDetail in memberDetails)
			{
				var memberScore = new MemberScore(memberDetail);
				memberScores.Add(memberDetail.tag, memberScore);
			}

			var leagueGroup = await GetAndDeserializeAsync<LeagueGroup>($"https://api.clashofclans.com/v1/clans/{clanTagEncoded}/currentwar/leaguegroup");
			if (leagueGroup == null)
			{
				return;
			}
			if (leagueGroup.clans == null)
			{
				var modelWar = await GetAndDeserializeAsync<Models.War>($"https://api.clashofclans.com/v1/clans/{clanTagEncoded}/currentwar");
				await TryReadWarAsync(null, modelWar, memberScores, leagueGroup);
			}
			else
			{
				foreach (var round in leagueGroup.rounds.EmptyIfNull())
				{
					foreach (var warTag in round.warTags.EmptyIfNull())
					{
						var warTagEncoded = WebUtility.UrlEncode(warTag);
						var modelWar = await GetAndDeserializeAsync<Models.War>($"https://api.clashofclans.com/v1/clanwarleagues/wars/{warTagEncoded}");
						if (modelWar == null)
						{
							continue;
						}
						await TryReadWarAsync(warTag, modelWar, memberScores, leagueGroup);
					}
				}
			}

			var utcNow = DateTimeOffset.UtcNow;
			var beginDate = new DateTimeOffset(utcNow.Year, utcNow.Month, 1, 0, 0, 0, TimeSpan.Zero);
			var endDate = beginDate.AddMonths(1);
			var players = await playerRepository.SelectForActivityAsync(false);
			var wars = await warRepository.SelectAsync(beginDate, endDate);
			var warIds = wars.Select(war => war.Id).ToList();
			var leagueWars = await leagueWarRepository.SelectAsync(warIds);
			List<PlayerScore> playerScores = new();
			List<string> headers = new();

			foreach (var player in players)
			{
				var playerScore = new PlayerScore { Name = player.Name };
				playerScores.Add(playerScore);
				playerScore.Donations = player.Donations;
				playerScore.DonationsReceived = player.DonationsReceived;

				var playerId = player.Id;
				var attacks = await attackRepository.SelectAsync(playerId, warIds);
				var playerWars = await warPlayerRepository.SelectAsync(playerId, warIds);
				var warAttacks = (
					from war in wars
					join playerWar in playerWars
					on war.Id equals playerWar.WarId
					join attack in attacks
					on new { WarId = war.Id, PlayerId = playerWar.PlayerId } equals new { WarId = attack.WarId, PlayerId = attack.PlayerId } into leftAttacks
					from leftAttack in leftAttacks.DefaultIfEmpty()
					select new { Attack = leftAttack, War = war })
					.OrderBy(warAttack => warAttack.War.StartTime)
					.ToList();

				foreach (var warAttack in warAttacks)
				{
					var war = warAttack.War;
					var startTime = war.StartTime;
					var isLeague = leagueWars.Any(leagueWar => leagueWar.WarId == war.Id);
					var playerAttack = playerScore.PlayerAttacks.FirstOrDefault(playerAttack => playerAttack.StartTime == startTime);
					if (playerAttack == null)
					{
						playerAttack = new PlayerAttack { StartTime = startTime, IsLeague = isLeague };
						playerScore.PlayerAttacks.Add(playerAttack);
					}
					var attack = warAttack.Attack;
					if (attack != null)
					{
						playerAttack.Battles.Add(new Battle(attack.Order, war.StartTime, attack.Stars, attack.MapPosition, attack.EnemyMapPosition, isLeague ? BattleTypeReference.League : BattleTypeReference.War));
					}
					else
					{
						playerAttack.Battles.Add(null);
					}
				}
			}

			if (fileName == null)
			{
				fileName = "clashResults";
			}
			fileName = Path.ChangeExtension(fileName, "csv");
			using (var writer = new StreamWriter(fileName, false, System.Text.Encoding.Unicode))
			using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
			{
				csv.WriteField("Nome");
				csv.WriteField("Pontos");
				csv.WriteField("Doações");
				csv.WriteField("Doações Recebidas");
				csv.WriteField("Total Estrelas");
				csv.WriteField("Total Penalização");

				var playerAttacks = playerScores
					.SelectMany(playerScore => playerScore.PlayerAttacks)
					.ToList();
				var startTimes = playerAttacks
					.Select(playerAttack => playerAttack.StartTime)
					.Distinct()
					.OrderBy(startTime => startTime)
					.ToList();

				Dictionary<DateTimeOffset, int> attackCount = new();
				int countField = 0;
				foreach (var startTime in startTimes)
				{
					var isLeague = playerAttacks.Any(playerAttack => playerAttack.StartTime == startTime && playerAttack.IsLeague);
					int numberOfAttacks = 1;
					var date = startTime.ToString("dd-MM-yyyy");
					var endColumnName = isLeague ? " Liga" : " 1º";
					csv.WriteField($"Estrelas {date}{endColumnName}");
					csv.WriteField($"Posição {++countField}");
					csv.WriteField($"Inimigo {countField}");
					csv.WriteField($"Penalização {countField}");
					if (!isLeague)
					{
						endColumnName = " 2º";
						csv.WriteField($"Estrelas {date}{endColumnName}");
						csv.WriteField($"Posição {++countField}");
						csv.WriteField($"Inimigo {countField}");
						csv.WriteField($"Penalização {countField}");
						numberOfAttacks = 2;
					}
					attackCount.Add(startTime, numberOfAttacks);
				}
				csv.NextRecord();
				foreach (var playerScore in playerScores.OrderByDescending(playerScore => playerScore.TotalScore))
				{
					csv.WriteField(playerScore.Name);
					csv.WriteField(playerScore.TotalScore);
					csv.WriteField(playerScore.Donations);
					csv.WriteField(playerScore.DonationsReceived);
					csv.WriteField(playerScore.TotalStars);
					csv.WriteField(playerScore.TotalPenalty);
					foreach (var startTime in startTimes)
					{
						var battles = playerScore.PlayerAttacks.Where(playerAttack => playerAttack.StartTime == startTime).FirstOrDefault()?.Battles;
						var count = attackCount[startTime];
						for (int index = 0; index < count; ++index)
						{
							var star = "";
							var battle = battles?.ElementAtOrDefault(index);
							if (battle != null)
							{
								if (battle == null)
								{
									star = "Não atacou";
								}
								else
								{
									star = battle.Stars.ToString();
								}
							}
							csv.WriteField(star);
							csv.WriteField($"{battle?.MapPosition}");
							csv.WriteField($"{battle?.EnemyMapPosition}");
							csv.WriteField($"{battle?.Penalty}");
						}
					}
					csv.NextRecord();
				}
			}
		}

		private async Task UpsertPlayerAsync(IEnumerable<MemberDetail> memberDetails)
		{
			var tags = memberDetails.Select(memberDetail => memberDetail.tag).ToList();
			var players = await playerRepository.SelectAsync(tags);
			foreach (var memberDetail in memberDetails)
			{
				var player = players.FirstOrDefault(player => player.Tag == memberDetail.tag);
				if (player == null)
				{
					player = new Player
					{
						Name = memberDetail.name,
						Tag = memberDetail.tag,
						Donations = memberDetail.donations,
						DonationsReceived = memberDetail.donationsReceived,
						HasLeft = false
					};
					await playerRepository.InsertAsync(player);
				}
				else if (
					player.Name != memberDetail.name ||
					player.Donations != memberDetail.donations ||
					player.DonationsReceived != memberDetail.donationsReceived ||
					player.HasLeft != false)
				{
					player.Name = memberDetail.name;
					player.Donations = memberDetail.donations;
					player.DonationsReceived = memberDetail.donationsReceived;
					player.HasLeft = false;
				}
			}
			await playerRepository.UpdateAsync(players);
			var playersThatHaveLeft = await playerRepository.SelectNotInAsync(tags, false);
			playersThatHaveLeft = playersThatHaveLeft.Select(player => { player.HasLeft = true; return player; }).ToList();
			await playerRepository.UpdateAsync(playersThatHaveLeft);
		}

		private async Task UpsertWarLeagueAsync(War war, Models.LeagueGroup leagueGroup)
		{
			if (leagueGroup.season == null)
			{
				return;
			}
			var league = await leagueRepository.SelectAsync(leagueGroup.season);
			if (league == null)
			{
				league = new League { Season = leagueGroup.season };
				await leagueRepository.InsertAsync(league);
			}
			var warLeague = await leagueWarRepository.SelectAsync(league.Id, war.Id);
			if (warLeague == null)
			{
				await leagueWarRepository.InsertAsync(new LeagueWar { LeagueId = league.Id, WarId = war.Id });
			}
		}

		private async Task<War> WarUpsertAsync(string? warTag, Models.War modelWar)
		{
			War? war;
			if (!string.IsNullOrWhiteSpace(warTag))
			{
				war = await warRepository.SelectAsync(warTag);
			}
			else
			{
				if (modelWar.preparationStartTime == null)
				{
					throw new Exception($"{nameof(modelWar.preparationStartTime)} can't be null");
				}
				war = await warRepository.SelectAsync(warTag, modelWar.preparationStartTime.Value);
			}
			if (war == null)
			{
				war = new War
				{
					Tag = warTag,
					EndTime = modelWar.endTime,
					PreparationStartTime = modelWar.preparationStartTime,
					StartTime = modelWar.startTime,
					State = modelWar.state,
					TeamSize = modelWar.teamSize
				};
				await warRepository.InsertAsync(war);
			}
			else
			{
				if (
					war.EndTime != modelWar.endTime ||
					war.PreparationStartTime != modelWar.preparationStartTime ||
					war.StartTime != modelWar.startTime ||
					war.State != modelWar.state ||
					war.TeamSize != modelWar.teamSize)
				{
					war.EndTime = modelWar.endTime;
					war.PreparationStartTime = modelWar.preparationStartTime;
					war.StartTime = modelWar.startTime;
					war.State = modelWar.state;
					war.TeamSize = modelWar.teamSize;

					await warRepository.UpdateAsync(war);
				}
			}
			return war;
		}

		public async Task<T?> GetAndDeserializeAsync<T>(string requestUri)
		{
			var response = await httpClient.GetAsync(requestUri);
			var content = response.Content;
			return JsonSerializer.Deserialize<T>(await content.ReadAsStreamAsync(), options);
		}

		private async Task<bool> TryReadWarAsync(string? warTag, Models.War? modelWar, Dictionary<string, MemberScore> memberScores, Models.LeagueGroup? leagueGroup)
		{
			if (modelWar == null || leagueGroup == null)
			{
				return false;
			}
			War war = await WarUpsertAsync(warTag, modelWar);
			await UpsertWarLeagueAsync(war, leagueGroup);
			var contender = modelWar.clan;
			var opponent = modelWar.opponent;
			if (contender == null || opponent == null)
			{
				return false;
			}
			Clan? clan;
			Clan? enemyClan;
			if (contender.tag == clanTag)
			{
				clan = contender;
				enemyClan = opponent;
			}
			else if (opponent.tag == clanTag)
			{
				clan = opponent;
				enemyClan = contender;
			}
			else
			{
				return false;
			}
			if (enemyClan.members == null)
			{
				return false;
			}
			foreach (var member in clan.members.EmptyIfNull())
			{
				if (member.tag == null || !memberScores.TryGetValue(member.tag, out MemberScore? memberScore))
				{
					continue;
				}
				await UpsertWarPlayerAsync(war, member);
				var atacks = member.attacks;
				if (atacks == null)
				{
					continue;
				}
				int index = 0;
				foreach (var atack in atacks)
				{
					if (atack == null)
					{
						continue;
					}
					var enemyMember = enemyClan.members.First(member => member.tag == atack.defenderTag);
					await UpsertAttackAsync(atack, war, member, enemyMember);
					if (enemyMember.mapPosition != null && member.mapPosition != null && modelWar.preparationStartTime != null)
					{
						memberScore.Battles.Add(new Battle(++index, modelWar.startTime, atack.stars, member.mapPosition.Value, enemyMember.mapPosition.Value, BattleTypeReference.League));
					}
				}
			}
			return true;
		}

		private async Task UpsertWarPlayerAsync(War war, Member member)
		{
			var player = (await playerRepository.SelectAsync(new[] { member.tag })).FirstOrDefault();
			if (player == null)
			{
				throw new Exception($"Player not found {nameof(member.tag)} = {member.tag}");
			}
			var warPlayer = (await warPlayerRepository.SelectAsync(player.Id, new[] { war.Id })).SingleOrDefault();
			if (warPlayer == null)
			{
				await warPlayerRepository.InsertAsync(new WarPlayer { PlayerId = player.Id, WarId = war.Id });
			}
		}

		private async Task UpsertAttackAsync(Models.Attack modelAtack, War war, Member member, Member enemyMember)
		{
			var player = (await playerRepository.SelectAsync(new[] { modelAtack.attackerTag })).FirstOrDefault();
			if (player == null)
			{
				return;
			}
			var attack = await attackRepository.SelectAsync(player.Id, war.Id, modelAtack.order);
			if (attack == null)
			{
				attack = new Attack
				{
					DefenderTag = modelAtack.defenderTag,
					DestructionPercentage = modelAtack.destructionPercentage,
					Duration = modelAtack.duration,
					Order = modelAtack.order,
					PlayerId = player.Id,
					Stars = modelAtack.stars,
					WarId = war.Id,
					MapPosition = member.mapPosition ?? -1,
					EnemyMapPosition = enemyMember.mapPosition ?? -1
				};
				await attackRepository.InsertAsync(attack);
			}
			else
			{
				if (
					attack.DefenderTag != modelAtack.defenderTag ||
					attack.DestructionPercentage != modelAtack.destructionPercentage ||
					attack.Duration != modelAtack.duration ||
					attack.Order != modelAtack.order ||
					attack.Stars != modelAtack.stars ||
					attack.MapPosition != member.mapPosition ||
					attack.EnemyMapPosition != enemyMember.mapPosition ||
					attack.WarId != war.Id)
				{
					attack.DefenderTag = modelAtack.defenderTag;
					attack.DestructionPercentage = modelAtack.destructionPercentage;
					attack.Duration = modelAtack.duration;
					attack.Order = modelAtack.order;
					attack.Stars = modelAtack.stars;
					attack.MapPosition = member.mapPosition ?? -1;
					attack.EnemyMapPosition = enemyMember.mapPosition ?? -1;
					attack.WarId = war.Id;
					await attackRepository.UpdateAsync(attack);
				}
			}
		}
	}
}
