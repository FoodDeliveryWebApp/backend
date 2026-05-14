-- UserRole stored as int: Administrator=0, Manager=1, Worker=2, DeliveryMan=3, Guest=4
INSERT INTO stakeholders."Users" ("Id", "Username", "Password", "Role", "IsActive") VALUES
(-1,  'admin@gmail.com',     'admin',     0, true),
(-2,  'manager1@gmail.com',  'manager1',  1, true),
(-3,  'manager2@gmail.com',  'manager2',  1, true),
(-4,  'worker1@gmail.com',   'worker1',   2, true),
(-5,  'worker2@gmail.com',   'worker2',   2, true),
(-6,  'delivery1@gmail.com', 'delivery1', 3, true),
(-21, 'turista1@gmail.com',  'turista1',  4, true),
(-22, 'turista2@gmail.com',  'turista2',  4, true),
(-23, 'turista3@gmail.com',  'turista3',  4, true);
