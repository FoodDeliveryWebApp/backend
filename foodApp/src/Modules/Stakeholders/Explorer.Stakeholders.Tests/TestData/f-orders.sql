-- OrderStatus stored as string: 'PickUp', 'Delivery', 'Preparing', 'Rejected', 'Delivered'
-- TotalPrice is computed (not stored); Note is optional
INSERT INTO stakeholders."Orders" ("Id", "UserId", "OrderTime", "Status", "Note") VALUES
(-1, -21, '2024-03-01 12:00:00+00', 'Delivered',  'Extra napkins please'),
(-2, -22, '2024-03-02 13:30:00+00', 'Delivered',  ''),
(-3, -21, '2024-03-03 19:00:00+00', 'Preparing',  'No onions'),
(-4, -23, '2024-03-04 20:15:00+00', 'PickUp',     ''),
(-5, -22, '2024-03-05 11:00:00+00', 'Rejected',   'Allergy to nuts');
