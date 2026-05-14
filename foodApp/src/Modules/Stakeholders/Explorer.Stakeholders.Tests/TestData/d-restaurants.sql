-- CuisineType stored as int: Italian=0, Chinese=1, Serbian=2, Indian=3, Mexican=4, American=5, Other=6
-- ManagerId is FK to Users
INSERT INTO stakeholders."Restaurants" ("Id", "Name", "Address", "PhoneNumber", "IsActive", "Cuisine", "ImageUrl", "ManagerId") VALUES
(-1, 'Pizzeria Roma',  'Bulevar Oslobodjenja 1, Novi Sad', '021-555-001', true, 0, 'https://example.com/roma.jpg',   -2),
(-2, 'Kineski Zid',    'Zmaj Jovina 10, Novi Sad',         '021-555-002', true, 1, 'https://example.com/china.jpg',  -3),
(-3, 'Srpska Kafana',  'Dunavska 5, Novi Sad',             '021-555-003', true, 2, 'https://example.com/kafana.jpg', -2);
