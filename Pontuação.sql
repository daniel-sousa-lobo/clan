;WITH PLayerCte(Id, Name, Donations, DonationsReceived)
AS
(
	SELECT
		Player.Id,
		Player.Name,
		Player.Donations,
		Player.DonationsReceived
	FROM
		Player
	WHERE
		Player.HasLeft = 0
),
AttackCte(DenseRank, RowNumber, PlayerId, StartTime, Stars)
AS
(
	SELECT
		DENSE_RANK() OVER (ORDER BY War.StartTime) DenseRank,
		ROW_NUMBER() OVER (PARTITION BY Attack.PlayerId, War.StartTime ORDER BY War.StartTime) RowNumber,
		Attack.PlayerId,
		FORMAT (War.StartTime, 'dd-MM-yyyy hh:mm:ss ') StartTime,
		Attack.Stars
	FROM
		War
	INNER JOIN
		Attack
	ON	Attack.WarId = War.Id
),
AttackCte2(DenseRank, RowNumber, PlayerId, StartTime, Stars)
AS
(
	SELECT
		DENSE_RANK() OVER (ORDER BY War.StartTime) DenseRank,
		ROW_NUMBER() OVER (PARTITION BY WarPlayer.PlayerId, War.StartTime ORDER BY War.StartTime) RowNumber,
		WarPlayer.PlayerId,
		FORMAT (War.StartTime, 'dd-MM-yyyy hh:mm:ss ') StartTime,
		Attack.Stars
	FROM
		War
	INNER JOIN
		WarPlayer
	ON	WarPlayer.WarId = War.Id
	LEFT JOIN
		Attack
	ON	Attack.WarId = War.Id AND
		Attack.PlayerId = WarPlayer.PlayerId
)
SELECT
	Classification.Nome,
	Classification.Estrelas + Classification.Penalização Pontos,
	Classification.Doações,
	Classification.[Doações Recebidas],
	Classification.Estrelas,
	Classification.Penalização,
	Classification.[Data 1],
	Classification.[Estrelas 1],
	Classification.[Data 2],
	Classification.[Estrelas 2],
	Classification.[Data 3],
	Classification.[Estrelas 3],
	Classification.[Data 4],
	Classification.[Estrelas 4],
	Classification.[Data 5],
	Classification.[Estrelas 5],
	Classification.[Data 6],
	Classification.[Estrelas 6],
	Classification.[Data 7],
	Classification.[Estrelas 7],
	Classification.[Data 8],
	Classification.[Estrelas 8.1],
	Classification.[Estrelas 8.2],
	Classification.[Data 9],
	Classification.[Estrelas 9.1],
	Classification.[Estrelas 9.2]
FROM
(
SELECT
	PLayerCte.Name [Nome],
	PLayerCte.Donations [Doações],
	PLayerCte.DonationsReceived [Doações Recebidas],
	CASE
		WHEN (PLayerCte.Donations - PLayerCte.DonationsReceived) / 250 < 0 THEN (PLayerCte.Donations - PLayerCte.DonationsReceived) / 250
		ELSE 0
	END +
	CASE WHEN AttackCte1.StartTime IS NOT NULL AND AttackCte1.Stars IS NULL THEN -1 ELSE 0 END +
	CASE WHEN AttackCte2.StartTime IS NOT NULL AND AttackCte2.Stars IS NULL THEN -1 ELSE 0 END +
	CASE WHEN AttackCte3.StartTime IS NOT NULL AND AttackCte3.Stars IS NULL THEN -1 ELSE 0 END +
	CASE WHEN AttackCte4.StartTime IS NOT NULL AND AttackCte4.Stars IS NULL THEN -1 ELSE 0 END +
	CASE WHEN AttackCte5.StartTime IS NOT NULL AND AttackCte5.Stars IS NULL THEN -1 ELSE 0 END +
	CASE WHEN AttackCte6.StartTime IS NOT NULL AND AttackCte6.Stars IS NULL THEN -1 ELSE 0 END +
	CASE WHEN AttackCte7.StartTime IS NOT NULL AND AttackCte7.Stars IS NULL THEN -1 ELSE 0 END +
	CASE WHEN AttackCte8_1.StartTime IS NOT NULL AND AttackCte8_1.Stars IS NULL THEN -1 ELSE 0 END +
	CASE WHEN AttackCte8_1.StartTime IS NOT NULL AND AttackCte8_2.Stars IS NULL THEN -1 ELSE 0 END +
	CASE WHEN AttackCte9_1.StartTime IS NOT NULL AND AttackCte9_1.Stars IS NULL THEN -1 ELSE 0 END +
	CASE WHEN AttackCte9_1.StartTime IS NOT NULL AND AttackCte9_2.Stars IS NULL THEN -1 ELSE 0 END [Penalização],
	ISNULL(AttackCte1.Stars, 0) +
	ISNULL(AttackCte2.Stars, 0) +
	ISNULL(AttackCte3.Stars, 0) +
	ISNULL(AttackCte4.Stars, 0) +
	ISNULL(AttackCte5.Stars, 0) +
	ISNULL(AttackCte6.Stars, 0) +
	ISNULL(AttackCte7.Stars, 0) +
	ISNULL(AttackCte8_1.Stars, 0) +
	ISNULL(AttackCte8_2.Stars, 0) +
	ISNULL(AttackCte9_1.Stars, 0) +
	ISNULL(AttackCte9_2.Stars, 0) [Estrelas],
	PLayerCte.Donations [Donativos],
	PLayerCte.DonationsReceived [Recebidos],
	AttackCte1.StartTime [Data 1],
	AttackCte1.Stars [Estrelas 1],
	AttackCte2.StartTime [Data 2],
	AttackCte2.Stars [Estrelas 2],
	AttackCte3.StartTime [Data 3],
	AttackCte3.Stars [Estrelas 3],
	AttackCte4.StartTime [Data 4],
	AttackCte4.Stars [Estrelas 4],
	AttackCte5.StartTime [Data 5],
	AttackCte5.Stars [Estrelas 5],
	AttackCte6.StartTime [Data 6],
	AttackCte6.Stars [Estrelas 6],
	AttackCte7.StartTime [Data 7],
	AttackCte7.Stars [Estrelas 7],
	AttackCte8_1.StartTime [Data 8],
	AttackCte8_1.Stars [Estrelas 8.1],
	AttackCte8_2.Stars [Estrelas 8.2],
	AttackCte9_1.StartTime [Data 9],
	AttackCte9_1.Stars [Estrelas 9.1],
	AttackCte9_2.Stars [Estrelas 9.2]
from PLayerCte
LEFT JOIN
	AttackCte AttackCte1
ON	AttackCte1.PlayerId = PLayerCte.Id AND
	AttackCte1.DenseRank = 1 AND
	AttackCte1.RowNumber = 1
LEFT JOIN
	AttackCte AttackCte2
ON	AttackCte2.PlayerId = PLayerCte.Id AND
	AttackCte2.DenseRank = 2 AND
	AttackCte2.RowNumber = 1
LEFT JOIN
	AttackCte AttackCte3
ON	AttackCte3.PlayerId = PLayerCte.Id AND
	AttackCte3.DenseRank = 3 AND
	AttackCte3.RowNumber = 1
LEFT JOIN
	AttackCte AttackCte4
ON	AttackCte4.PlayerId = PLayerCte.Id AND
	AttackCte4.DenseRank = 4 AND
	AttackCte4.RowNumber = 1
LEFT JOIN
	AttackCte AttackCte5
ON	AttackCte5.PlayerId = PLayerCte.Id AND
	AttackCte5.DenseRank = 5 AND
	AttackCte5.RowNumber = 1
LEFT JOIN
	AttackCte AttackCte6
ON	AttackCte6.PlayerId = PLayerCte.Id AND
	AttackCte6.DenseRank = 6 AND
	AttackCte6.RowNumber = 1
LEFT JOIN
	AttackCte AttackCte7
ON	AttackCte7.PlayerId = PLayerCte.Id AND
	AttackCte7.DenseRank = 7 AND
	AttackCte7.RowNumber = 1
LEFT JOIN
	AttackCte AttackCte8_1
ON	AttackCte8_1.PlayerId = PLayerCte.Id AND
	AttackCte8_1.DenseRank = 8 AND
	AttackCte8_1.RowNumber = 1
LEFT JOIN
	AttackCte AttackCte8_2
ON	AttackCte8_2.PlayerId = PLayerCte.Id AND
	AttackCte8_2.DenseRank = 8 AND
	AttackCte8_2.RowNumber = 2
LEFT JOIN
	AttackCte2 AttackCte9_1
ON	AttackCte9_1.PlayerId = PLayerCte.Id AND
	AttackCte9_1.DenseRank = 1 AND
	AttackCte9_1.RowNumber = 1
LEFT JOIN
	AttackCte2 AttackCte9_2
ON	AttackCte9_2.PlayerId = PLayerCte.Id AND
	AttackCte9_2.DenseRank = 1 AND
	AttackCte9_2.RowNumber = 2
) Classification
ORDER BY
	Classification.Estrelas + Classification.Penalização
DESC
