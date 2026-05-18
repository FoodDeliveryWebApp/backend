-- RatingReportStatus stored as string: 'Pending', 'Approved', 'Rejected'
-- RatingId and ManagerId are shadow FKs from navigation properties
INSERT INTO stakeholders."RatingReports" ("Id", "RatingId", "ManagerId", "Reason", "Status", "CreatedAt") VALUES
(-1, -1, -2, 'Guest reported an issue with order quality.',      'Pending',  '2024-03-02 09:00:00+00'),
(-2, -2, -3, 'Late delivery, compensation approved.',           'Approved', '2024-03-03 10:00:00+00'),
(-3, -5, -2, 'Order rejected due to allergen miscommunication.','Rejected', '2024-03-06 08:00:00+00');
