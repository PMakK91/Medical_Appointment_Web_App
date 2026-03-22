SET NOCOUNT ON;

DECLARE @Now DATETIME2 = SYSUTCDATETIME();

------------------------------------------------------------
-- 1) DOCTORS
------------------------------------------------------------
DECLARE @Doctors TABLE
(
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    email NVARCHAR(254),
    amka NVARCHAR(11),
    gender INT,
    phone NVARCHAR(50),
    birth_date DATE,
    [password] NVARCHAR(MAX),
    specialty INT,
    license_number NVARCHAR(100),
    bio NVARCHAR(500),
    image NVARCHAR(MAX),
    registration_status INT,
    full_address NVARCHAR(MAX),
    lon FLOAT,
    lat FLOAT,
    duration INT,
    monday_start TIME NULL,
    monday_end TIME NULL,
    tuesday_start TIME NULL,
    tuesday_end TIME NULL,
    wednesday_start TIME NULL,
    wednesday_end TIME NULL,
    thursday_start TIME NULL,
    thursday_end TIME NULL,
    friday_start TIME NULL,
    friday_end TIME NULL,
    saturday_start TIME NULL,
    saturday_end TIME NULL,
    sunday_start TIME NULL,
    sunday_end TIME NULL
);

INSERT INTO @Doctors VALUES
(N'Nikolaos', N'Papadopoulos', N'nikolaos.papadopoulos@d.gr', N'12345678901', 0, N'6900000001', '1980-05-12', N'Password123!', 3,  N'LIC-001', N'Καρδιολόγος με έμφαση στην προληπτική καρδιολογία.', N'd1.jpg',  1, N'Λεωφόρος Βασιλίσσης Σοφίας 80, Αθήνα 11528', 23.7546, 37.9806, 30, '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '14:00', NULL, NULL, NULL, NULL),
(N'Maria', N'Georgiou', N'maria.georgiou@d.gr', N'12345678902', 1, N'6900000002', '1985-02-18', N'Password123!', 7,  N'LIC-002', N'Δερματολόγος με εμπειρία σε κλινική και αισθητική δερματολογία.', N'd2.jpg',  1, N'Μητροπόλεως 6, Αθήνα 10563', 23.7331, 37.9753, 20, '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '16:00', NULL, NULL, NULL, NULL),
(N'Dimitrios', N'Ioannou', N'dimitrios.ioannou@d.gr', N'12345678903', 0, N'6900000003', '1978-07-04', N'Password123!', 33, N'LIC-003', N'Ορθοπεδικός χειρουργός με εξειδίκευση σε αθλητικές κακώσεις.', N'd3.jpg',  1, N'Λεωφόρος Κηφισίας 7, Αθήνα 11523', 23.7619, 37.9875, 30, '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '13:30', NULL, NULL, NULL, NULL),
(N'Eleni', N'Karagianni', N'eleni.karagianni@d.gr', N'12345678904', 1, N'6900000004', '1983-09-21', N'Password123!', 18, N'LIC-004', N'Γυναικολόγος με έμφαση στην προγεννητική φροντίδα.', N'd4.jpg',  1, N'Πανόρμου 115, Αθήνα 11524', 23.7658, 37.9932, 30, '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '15:00', NULL, NULL, NULL, NULL),
(N'Georgios', N'Nikolaidis', N'georgios.nikolaidis@d.gr', N'12345678905', 0, N'6900000005', '1976-11-09', N'Password123!', 44, N'LIC-005', N'Πνευμονολόγος με εμπειρία σε χρόνιες αναπνευστικές παθήσεις.', N'd5.jpg',  1, N'Πατησίων 123, Αθήνα 11251', 23.7303, 38.0018, 25, '09:00', '14:00', '09:00', '14:00', '12:00', '19:00', '09:00', '14:00', '09:00', '14:00', NULL, NULL, NULL, NULL),
(N'Sofia', N'Christodoulou', N'sofia.christodoulou@d.gr', N'12345678906', 1, N'6900000006', '1988-01-30', N'Password123!', 41, N'LIC-006', N'Ψυχίατρος ενηλίκων με εξατομικευμένη προσέγγιση.', N'd6.jpg',  1, N'Σόλωνος 74, Αθήνα 10680', 23.7399, 37.9836, 45, '11:00', '19:00', '11:00', '19:00', '11:00', '19:00', '11:00', '19:00', '11:00', '17:00', NULL, NULL, NULL, NULL),
(N'Panagiotis', N'Alexiou', N'panagiotis.alexiou@d.gr', N'12345678907', 0, N'6900000007', '1979-06-15', N'Password123!', 26, N'LIC-007', N'Νευρολόγος με κλινική εμπειρία σε κεφαλαλγίες και νευροπάθειες.', N'd7.jpg',  1, N'Λεωφόρος Μεσογείων 264, Αθήνα 15562', 23.8139, 37.9994, 30, '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '14:30', NULL, NULL, NULL, NULL),
(N'Katerina', N'Mavridou', N'katerina.mavridou@d.gr', N'12345678908', 1, N'6900000008', '1984-03-02', N'Password123!', 12, N'LIC-008', N'Ενδοκρινολόγος με εμπειρία σε θυρεοειδή και μεταβολισμό.', N'd8.jpg',  1, N'Ερμού 18, Αθήνα 10563', 23.7295, 37.9767, 30, '10:00', '17:00', '10:00', '17:00', '10:00', '17:00', '10:00', '17:00', '10:00', '15:00', NULL, NULL, NULL, NULL),
(N'Ioannis', N'Vlachos', N'ioannis.vlachos@d.gr', N'12345678909', 0, N'6900000009', '1981-12-14', N'Password123!', 23, N'LIC-009', N'Παθολόγος με ενδιαφέρον στην πρωτοβάθμια φροντίδα.', N'd9.jpg',  1, N'Αχαρνών 120, Αθήνα 10434', 23.7319, 37.9920, 20, '08:00', '14:00', '08:00', '14:00', '08:00', '14:00', '08:00', '14:00', '08:00', '13:00', NULL, NULL, NULL, NULL),
(N'Anna', N'Rapti', N'anna.rapti@d.gr', N'12345678910', 1, N'6900000010', '1987-08-26', N'Password123!', 31, N'LIC-010', N'Οφθαλμίατρος με έμφαση σε προληπτικούς ελέγχους.', N'd10.jpg', 1, N'Λεωφόρος Συγγρού 196, Καλλιθέα 17671', 23.7126, 37.9577, 25, '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '14:00', NULL, NULL, NULL, NULL),

(N'Thanos', N'Kostopoulos', N'thanos.kostopoulos@d.gr', N'12345678911', 0, N'6900000011', '1977-04-01', N'Password123!', 53, N'LIC-011', N'Ουρολόγος με εμπειρία σε λιθίαση και ανδρική υγεία.', N'd11.jpg', 1, N'Ηρώων Πολυτεχνείου 91, Πειραιάς 18536', 23.6465, 37.9413, 30, '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '15:00', NULL, NULL, NULL, NULL),
(N'Chrysa', N'Lamprou', N'chrysa.lamprou@d.gr', N'12345678912', 1, N'6900000012', '1986-10-06', N'Password123!', 47, N'LIC-012', N'Ρευματολόγος με έμφαση στα αυτοάνοσα νοσήματα.', N'd12.jpg', 1, N'Λεωφόρος Δημάρχου Αγγέλου Μεταξά 24, Γλυφάδα 16674', 23.7515, 37.8646, 30, '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '14:30', NULL, NULL, NULL, NULL),
(N'Manolis', N'Sarris', N'manolis.sarris@d.gr', N'12345678913', 0, N'6900000013', '1975-05-20', N'Password123!', 16, N'LIC-013', N'Γενικός χειρουργός με εμπειρία σε λαπαροσκοπικές επεμβάσεις.', N'd13.jpg', 1, N'Λεωφόρος Κηφισίας 49, Μαρούσι 15123', 23.8008, 38.0437, 40, '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '13:30', NULL, NULL, NULL, NULL),
(N'Iro', N'Tzima', N'iro.tzima@d.gr', N'12345678914', 1, N'6900000014', '1989-07-07', N'Password123!', 42, N'LIC-014', N'Ψυχολόγος με εμπειρία σε άγχος και συμβουλευτική ενηλίκων.', N'd14.jpg', 1, N'Κολοκοτρώνη 15, Κηφισιά 14562', 23.8082, 38.0746, 50, '12:00', '20:00', '12:00', '20:00', '12:00', '20:00', '12:00', '20:00', '12:00', '17:00', NULL, NULL, NULL, NULL),
(N'Petros', N'Markou', N'petros.markou@d.gr', N'12345678915', 0, N'6900000015', '1982-01-19', N'Password123!', 34, N'LIC-015', N'ΩΡΛ με εμπειρία σε αλλεργίες και παιδιατρικά περιστατικά.', N'd15.jpg', 1, N'Ανδρέα Παπανδρέου 35, Χαλάνδρι 15232', 23.7981, 38.0217, 25, '10:00', '17:00', '10:00', '17:00', '10:00', '17:00', '10:00', '17:00', '10:00', '14:00', NULL, NULL, NULL, NULL),
(N'Foteini', N'Apostolou', N'foteini.apostolou@d.gr', N'12345678916', 1, N'6900000016', '1983-02-11', N'Password123!', 15, N'LIC-016', N'Γαστρεντερολόγος με έμφαση στους προληπτικούς ελέγχους.', N'd16.jpg', 1, N'Λεωφόρος Μεσογείων 395, Αγία Παρασκευή 15343', 23.8162, 38.0089, 30, '09:00', '16:00', '09:00', '16:00', '09:00', '16:00', '09:00', '16:00', '09:00', '14:00', NULL, NULL, NULL, NULL),
(N'Stavros', N'Papanikolaou', N'stavros.papanikolaou@d.gr', N'12345678917', 0, N'6900000017', '1974-09-29', N'Password123!', 25, N'LIC-017', N'Νεφρολόγος με εμπειρία στην υπέρταση και τη ΧΝΝ.', N'd17.jpg', 1, N'Εθνικής Αντιστάσεως 62, Περιστέρι 12134', 23.6924, 38.0154, 30, '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '13:30', NULL, NULL, NULL, NULL),
(N'Vasiliki', N'Drougka', N'vasiliki.drougka@d.gr', N'12345678918', 1, N'6900000018', '1990-06-08', N'Password123!', 37, N'LIC-018', N'Φυσικοθεραπεύτρια με έμφαση στη μυοσκελετική αποκατάσταση.', N'd18.jpg', 1, N'Θηβών 210, Ίλιον 13122', 23.6997, 38.0314, 30, '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '15:00', '10:00', '13:00', NULL, NULL),
(N'Christos', N'Zervas', N'christos.zervas@d.gr', N'12345678919', 0, N'6900000019', '1981-03-03', N'Password123!', 6,  N'LIC-019', N'Οδοντίατρος με εμπειρία στην αισθητική οδοντιατρική.', N'd19.jpg', 1, N'Αγίου Κωνσταντίνου 12, Αιγάλεω 12243', 23.6794, 37.9936, 30, '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '16:00', NULL, NULL, NULL, NULL),
(N'Despina', N'Ralli', N'despina.ralli@d.gr', N'12345678920', 1, N'6900000020', '1988-12-22', N'Password123!', 9,  N'LIC-020', N'Διαιτολόγος με στόχο την εξατομικευμένη διατροφική υποστήριξη.', N'd20.jpg', 1, N'Λεωφόρος Ελευθερίου Βενιζέλου 120, Νέα Σμύρνη 17122', 23.7163, 37.9451, 40, '09:00', '16:00', '09:00', '16:00', '09:00', '16:00', '09:00', '16:00', '09:00', '14:00', NULL, NULL, NULL, NULL),

(N'Konstantinos', N'Anagnostou', N'konstantinos.anagnostou@d.gr', N'12345678921', 0, N'6900000021', '1979-09-10', N'Password123!', 3,  N'LIC-021', N'Καρδιολόγος με έμφαση στην καρδιακή ανεπάρκεια.', N'd21.jpg', 1, N'Εγνατία 90, Θεσσαλονίκη 54624', 22.9444, 40.6342, 30, '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '14:00', NULL, NULL, NULL, NULL),
(N'Evangelia', N'Pavlidou', N'evangelia.pavlidou@d.gr', N'12345678922', 1, N'6900000022', '1986-05-17', N'Password123!', 7,  N'LIC-022', N'Δερματολόγος για κλινική και αισθητική δερματολογία.', N'd22.jpg', 1, N'Τσιμισκή 122, Θεσσαλονίκη 54621', 22.9496, 40.6298, 20, '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '16:00', NULL, NULL, NULL, NULL),
(N'Alexandros', N'Filippidis', N'alexandros.filippidis@d.gr', N'12345678923', 0, N'6900000023', '1980-08-08', N'Password123!', 44, N'LIC-023', N'Πνευμονολόγος με εμπειρία σε άσθμα και ΧΑΠ.', N'd23.jpg', 1, N'Μητροπόλεως 45, Θεσσαλονίκη 54623', 22.9469, 40.6326, 25, '09:00', '14:00', '09:00', '14:00', '12:00', '19:00', '09:00', '14:00', '09:00', '14:00', NULL, NULL, NULL, NULL),
(N'Georgia', N'Koutrou', N'georgia.koutrou@d.gr', N'12345678924', 1, N'6900000024', '1987-11-11', N'Password123!', 18, N'LIC-024', N'Γυναικολόγος με επίκεντρο την προληπτική ιατρική.', N'd24.jpg', 1, N'Αγίας Σοφίας 28, Θεσσαλονίκη 54622', 22.9478, 40.6320, 30, '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '15:00', NULL, NULL, NULL, NULL),
(N'Ioanna', N'Baka', N'ioanna.baka@d.gr', N'12345678925', 1, N'6900000025', '1989-01-25', N'Password123!', 42, N'LIC-025', N'Ψυχολόγος με έμφαση στη διαχείριση άγχους.', N'd25.jpg', 1, N'Βασιλίσσης Όλγας 120, Θεσσαλονίκη 54645', 22.9608, 40.6119, 50, '12:00', '20:00', '12:00', '20:00', '12:00', '20:00', '12:00', '20:00', '12:00', '17:00', NULL, NULL, NULL, NULL),

(N'Andreas', N'Kanelopoulos', N'andreas.kanelopoulos@d.gr', N'12345678926', 0, N'6900000026', '1978-04-14', N'Password123!', 16, N'LIC-026', N'Γενικός χειρουργός με εμπειρία σε επεμβατική χειρουργική.', N'd26.jpg', 1, N'Μαιζώνος 110, Πάτρα 26221', 21.7346, 38.2466, 40, '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '14:30', '08:30', '13:30', NULL, NULL, NULL, NULL),
(N'Rania', N'Petraki', N'rania.petraki@d.gr', N'12345678927', 1, N'6900000027', '1985-06-19', N'Password123!', 12, N'LIC-027', N'Ενδοκρινολόγος με έμφαση στον σακχαρώδη διαβήτη.', N'd27.jpg', 1, N'Κορίνθου 220, Πάτρα 26221', 21.7315, 38.2440, 30, '10:00', '17:00', '10:00', '17:00', '10:00', '17:00', '10:00', '17:00', '10:00', '15:00', NULL, NULL, NULL, NULL),
(N'Michalis', N'Kritikos', N'michalis.kritikos@d.gr', N'12345678928', 0, N'6900000028', '1977-02-27', N'Password123!', 31, N'LIC-028', N'Οφθαλμίατρος με εμπειρία σε παθήσεις αμφιβληστροειδούς.', N'd28.jpg', 1, N'25ης Αυγούστου 12, Ηράκλειο 71202', 25.1362, 35.3387, 25, '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '15:00', '09:00', '14:00', NULL, NULL, NULL, NULL),
(N'Elisavet', N'Manousaki', N'elisavet.manousaki@d.gr', N'12345678929', 1, N'6900000029', '1988-09-04', N'Password123!', 41, N'LIC-029', N'Ψυχίατρος ενηλίκων με εμπειρία στην ψυχοφαρμακολογία.', N'd29.jpg', 1, N'Λεωφόρος Κνωσού 44, Ηράκλειο 71306', 25.1549, 35.3275, 45, '11:00', '19:00', '11:00', '19:00', '11:00', '19:00', '11:00', '19:00', '11:00', '17:00', NULL, NULL, NULL, NULL),
(N'Georgios', N'Tsakalos', N'georgios.tsakalos@d.gr', N'12345678930', 0, N'6900000030', '1982-12-01', N'Password123!', 53, N'LIC-030', N'Ουρολόγος με εμπειρία σε λειτουργικές παθήσεις ουροποιητικού.', N'd30.jpg', 1, N'Παπαναστασίου 60, Λάρισα 41222', 22.4190, 39.6363, 30, '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '18:00', '10:00', '15:00', NULL, NULL, NULL, NULL),
(N'Natalia', N'Kara', N'natalia.kara@d.gr', N'12345678931', 1, N'6900000031', '1990-03-16', N'Password123!', 37, N'LIC-031', N'Φυσικοθεραπεύτρια με έμφαση στην αθλητική αποκατάσταση.', N'd31.jpg', 1, N'Ιάσονος 87, Βόλος 38221', 22.9424, 39.3616, 30, '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '17:00', '09:00', '15:00', '10:00', '13:00', NULL, NULL),
(N'Antonis', N'Zois', N'antonis.zois@d.gr', N'12345678932', 0, N'6900000032', '1976-07-28', N'Password123!', 26, N'LIC-032', N'Νευρολόγος με ενδιαφέρον σε μνήμη και κινητικές διαταραχές.', N'd32.jpg', 1, N'Ανεξαρτησίας 25, Ιωάννινα 45444', 20.8522, 39.6650, 30, '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '14:30', NULL, NULL, NULL, NULL),
(N'Christina', N'Fotiadou', N'christina.fotiadou@d.gr', N'12345678933', 1, N'6900000033', '1987-10-12', N'Password123!', 47, N'LIC-033', N'Ρευματολόγος με εμπειρία στην οστεοπόρωση και τις αρθρίτιδες.', N'd33.jpg', 1, N'Ερυθρού Σταυρού 8, Καβάλα 65403', 24.4125, 40.9396, 30, '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '16:30', '09:30', '14:30', NULL, NULL, NULL, NULL);

------------------------------------------------------------
-- Insert users for doctors if not exists
------------------------------------------------------------
INSERT INTO users
(
    id, first_name, last_name, email, amka, gender, phone, birth_date,
    [password], registration_date, role, refresh_token, refresh_token_expiry_time
)
SELECT
    NEWID(),
    d.first_name,
    d.last_name,
    d.email,
    d.amka,
    d.gender,
    d.phone,
    d.birth_date,
    d.[password],
    @Now,
    1,
    NULL,
    NULL
FROM @Doctors d
WHERE NOT EXISTS
(
    SELECT 1
    FROM users u
    WHERE u.email = d.email
);

------------------------------------------------------------
-- Insert doctors if not exists
------------------------------------------------------------
INSERT INTO doctors
(
    id, image, bio, specialty, license_number, registration_status
)
SELECT
    u.id,
    d.image,
    d.bio,
    d.specialty,
    d.license_number,
    d.registration_status
FROM @Doctors d
JOIN users u ON u.email = d.email
LEFT JOIN doctors doc ON doc.id = u.id
WHERE doc.id IS NULL;

------------------------------------------------------------
-- Insert primary practice for each doctor if not exists
------------------------------------------------------------
INSERT INTO practices
(
    id, doctor_id, duration, lon, lat, full_address, phone,
    is_deleted, deleted_time,
    monday_start, monday_end,
    tuesday_start, tuesday_end,
    wednesday_start, wednesday_end,
    thursday_start, thursday_end,
    friday_start, friday_end,
    saturday_start, saturday_end,
    sunday_start, sunday_end,
    registration_status, registration_date
)
SELECT
    NEWID(),
    u.id,
    d.duration,
    d.lon,
    d.lat,
    d.full_address,
    d.phone,
    0,
    NULL,
    d.monday_start, d.monday_end,
    d.tuesday_start, d.tuesday_end,
    d.wednesday_start, d.wednesday_end,
    d.thursday_start, d.thursday_end,
    d.friday_start, d.friday_end,
    d.saturday_start, d.saturday_end,
    d.sunday_start, d.sunday_end,
    d.registration_status,
    @Now
FROM @Doctors d
JOIN users u ON u.email = d.email
LEFT JOIN practices p
    ON p.doctor_id = u.id
   AND p.full_address = d.full_address
WHERE p.id IS NULL;

------------------------------------------------------------
-- Extra second practices for some doctors
------------------------------------------------------------
DECLARE @ExtraPractices TABLE
(
    doctor_email NVARCHAR(254),
    full_address NVARCHAR(MAX),
    lon FLOAT,
    lat FLOAT,
    phone NVARCHAR(50),
    duration INT,
    monday_start TIME NULL,
    monday_end TIME NULL,
    tuesday_start TIME NULL,
    tuesday_end TIME NULL,
    wednesday_start TIME NULL,
    wednesday_end TIME NULL,
    thursday_start TIME NULL,
    thursday_end TIME NULL,
    friday_start TIME NULL,
    friday_end TIME NULL,
    saturday_start TIME NULL,
    saturday_end TIME NULL,
    sunday_start TIME NULL,
    sunday_end TIME NULL,
    registration_status INT
);

INSERT INTO @ExtraPractices VALUES
(N'nikolaos.papadopoulos@d.gr', N'Λεωφόρος Ποσειδώνος 42, Παλαιό Φάληρο 17561', 23.7008, 37.9282, N'6901000001', 30, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, '16:00', '20:00', '10:00', '13:00', NULL, NULL, 1),
(N'maria.georgiou@d.gr', N'Λεωφόρος Κηφισίας 215, Μαρούσι 15124', 23.8057, 38.0378, N'6901000002', 20, NULL, NULL, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, '10:00', '14:00', NULL, NULL, 1),
(N'dimitrios.ioannou@d.gr', N'Θησέως 280, Καλλιθέα 17675', 23.7069, 37.9548, N'6901000003', 30, NULL, NULL, NULL, NULL, '16:00', '20:00', NULL, NULL, '16:00', '20:00', '10:00', '13:00', NULL, NULL, 1),
(N'eleni.karagianni@d.gr', N'Λεωφόρος Ελευθερίου Βενιζέλου 180, Νέα Σμύρνη 17121', 23.7182, 37.9501, N'6901000004', 30, '17:30', '20:30', NULL, NULL, '17:30', '20:30', NULL, NULL, '17:30', '20:30', NULL, NULL, NULL, NULL, 1),
(N'sofia.christodoulou@d.gr', N'Κηφισίας 58, Μαρούσι 15125', 23.8025, 38.0449, N'6901000005', 45, NULL, NULL, '10:00', '14:00', NULL, NULL, '10:00', '14:00', NULL, NULL, NULL, NULL, NULL, NULL, 1),
(N'petros.markou@d.gr', N'Αρτέμιδος 3, Παλλήνη 15351', 23.8838, 38.0064, N'6901000006', 25, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, NULL, NULL, '10:00', '13:00', NULL, NULL, 1),
(N'konstantinos.anagnostou@d.gr', N'Βασιλίσσης Όλγας 185, Θεσσαλονίκη 54646', 22.9629, 40.6088, N'6901000007', 30, NULL, NULL, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, '10:00', '13:00', NULL, NULL, 1),
(N'evangelia.pavlidou@d.gr', N'Καραολή και Δημητρίου 18, Εύοσμος 56224', 22.9056, 40.6685, N'6901000008', 20, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1),
(N'andreas.kanelopoulos@d.gr', N'Γούναρη 76, Πάτρα 26225', 21.7340, 38.2460, N'6901000009', 40, NULL, NULL, NULL, NULL, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, NULL, NULL, 1),
(N'michalis.kritikos@d.gr', N'Λεωφόρος 62 Μαρτύρων 146, Ηράκλειο 71303', 25.1168, 35.3331, N'6901000010', 25, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, NULL, NULL, '10:00', '13:00', NULL, NULL, 1),
(N'georgios.tsakalos@d.gr', N'Κούμα 12, Λάρισα 41222', 22.4164, 39.6390, N'6901000011', 30, NULL, NULL, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, NULL, NULL, NULL, NULL, 1),
(N'antonis.zois@d.gr', N'Δωδώνης 21, Ιωάννινα 45221', 20.8495, 39.6632, N'6901000012', 30, '16:00', '20:00', NULL, NULL, '16:00', '20:00', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1);

INSERT INTO practices
(
    id, doctor_id, duration, lon, lat, full_address, phone,
    is_deleted, deleted_time,
    monday_start, monday_end,
    tuesday_start, tuesday_end,
    wednesday_start, wednesday_end,
    thursday_start, thursday_end,
    friday_start, friday_end,
    saturday_start, saturday_end,
    sunday_start, sunday_end,
    registration_status, registration_date
)
SELECT
    NEWID(),
    u.id,
    ep.duration,
    ep.lon,
    ep.lat,
    ep.full_address,
    ep.phone,
    0,
    NULL,
    ep.monday_start, ep.monday_end,
    ep.tuesday_start, ep.tuesday_end,
    ep.wednesday_start, ep.wednesday_end,
    ep.thursday_start, ep.thursday_end,
    ep.friday_start, ep.friday_end,
    ep.saturday_start, ep.saturday_end,
    ep.sunday_start, ep.sunday_end,
    ep.registration_status,
    @Now
FROM @ExtraPractices ep
JOIN users u ON u.email = ep.doctor_email
LEFT JOIN practices p
    ON p.doctor_id = u.id
   AND p.full_address = ep.full_address
WHERE p.id IS NULL;

------------------------------------------------------------
-- 2) PATIENTS
------------------------------------------------------------
DECLARE @Patients TABLE
(
    first_name NVARCHAR(100),
    last_name NVARCHAR(100),
    email NVARCHAR(254),
    amka NVARCHAR(11),
    gender INT,
    phone NVARCHAR(50),
    birth_date DATE,
    [password] NVARCHAR(MAX)
);

INSERT INTO @Patients VALUES
(N'Giorgos',   N'Pappas',       N'giorgos.pappas@p.gr',        N'22345678901', 0, N'6911000001', '1993-01-10', N'Password123!'),
(N'Eirini',    N'Nikolaou',     N'eirini.nikolaou@p.gr',       N'22345678902', 1, N'6911000002', '1995-04-12', N'Password123!'),
(N'Christos',  N'Antonopoulos', N'christos.antonopoulos@p.gr', N'22345678903', 0, N'6911000003', '1990-08-22', N'Password123!'),
(N'Marina',    N'Kotsou',       N'marina.kotsou@p.gr',         N'22345678904', 1, N'6911000004', '1998-03-14', N'Password123!'),
(N'Vasilis',   N'Doukas',       N'vasilis.doukas@p.gr',        N'22345678905', 0, N'6911000005', '1989-12-02', N'Password123!'),
(N'Anna',      N'Lioliou',      N'anna.lioliou@p.gr',          N'22345678906', 1, N'6911000006', '1997-07-19', N'Password123!'),
(N'Petros',    N'Galanis',      N'petros.galanis@p.gr',        N'22345678907', 0, N'6911000007', '1987-09-01', N'Password123!'),
(N'Rania',     N'Konti',        N'rania.konti@p.gr',           N'22345678908', 1, N'6911000008', '1992-11-25', N'Password123!'),
(N'Kostas',    N'Vrettos',      N'kostas.vrettos@p.gr',        N'22345678909', 0, N'6911000009', '1991-05-05', N'Password123!'),
(N'Sofia',     N'Panou',        N'sofia.panou@p.gr',           N'22345678910', 1, N'6911000010', '1999-06-29', N'Password123!'),
(N'Dimitra',   N'Kyriazidou',   N'dimitra.kyriazidou@p.gr',    N'22345678911', 1, N'6911000011', '1988-02-16', N'Password123!'),
(N'Michalis',  N'Roussos',      N'michalis.roussos@p.gr',      N'22345678912', 0, N'6911000012', '1994-10-30', N'Password123!'),
(N'Ioanna',    N'Kourti',       N'ioanna.kourti@p.gr',         N'22345678913', 1, N'6911000013', '1996-01-07', N'Password123!'),
(N'Tasos',     N'Leventis',     N'tasos.leventis@p.gr',        N'22345678914', 0, N'6911000014', '1986-04-04', N'Password123!'),
(N'Eleni',     N'Stamatou',     N'eleni.stamatou@p.gr',        N'22345678915', 1, N'6911000015', '1993-12-18', N'Password123!'),
(N'Antonis',   N'Karalis',      N'antonis.karalis@p.gr',       N'22345678916', 0, N'6911000016', '1990-03-03', N'Password123!'),
(N'Katerina',  N'Zerva',        N'katerina.zerva@p.gr',        N'22345678917', 1, N'6911000017', '1997-08-11', N'Password123!'),
(N'Giannis',   N'Mpasios',      N'giannis.mpasios@p.gr',       N'22345678918', 0, N'6911000018', '1985-09-21', N'Password123!'),
(N'Natalia',   N'Dima',         N'natalia.dima@p.gr',          N'22345678919', 1, N'6911000019', '1998-05-27', N'Password123!'),
(N'Spyros',    N'Tzortzis',     N'spyros.tzortzis@p.gr',       N'22345678920', 0, N'6911000020', '1992-07-09', N'Password123!');

INSERT INTO users
(
    id, first_name, last_name, email, amka, gender, phone, birth_date,
    [password], registration_date, role, refresh_token, refresh_token_expiry_time
)
SELECT
    NEWID(),
    p.first_name,
    p.last_name,
    p.email,
    p.amka,
    p.gender,
    p.phone,
    p.birth_date,
    p.[password],
    @Now,
    2,
    NULL,
    NULL
FROM @Patients p
WHERE NOT EXISTS
(
    SELECT 1
    FROM users u
    WHERE u.email = p.email
);

INSERT INTO patients (id)
SELECT u.id
FROM @Patients p
JOIN users u ON u.email = p.email
LEFT JOIN patients pa ON pa.id = u.id
WHERE pa.id IS NULL;

------------------------------------------------------------
-- 3) APPOINTMENTS
------------------------------------------------------------
IF OBJECT_ID('tempdb..#DoctorsPractices') IS NOT NULL DROP TABLE #DoctorsPractices;
SELECT
    p.id AS practice_id,
    p.doctor_id,
    p.duration,
    p.monday_start, p.monday_end,
    p.tuesday_start, p.tuesday_end,
    p.wednesday_start, p.wednesday_end,
    p.thursday_start, p.thursday_end,
    p.friday_start, p.friday_end,
    p.saturday_start, p.saturday_end,
    p.sunday_start, p.sunday_end
INTO #DoctorsPractices
FROM practices p
JOIN doctors d ON d.id = p.doctor_id
WHERE p.is_deleted = 0;

IF OBJECT_ID('tempdb..#PatientsSeed') IS NOT NULL DROP TABLE #PatientsSeed;
SELECT
    ROW_NUMBER() OVER (ORDER BY u.email) AS rn,
    u.id AS patient_id
INTO #PatientsSeed
FROM users u
JOIN patients p ON p.id = u.id;

DECLARE
    @practice_id UNIQUEIDENTIFIER,
    @doctor_id UNIQUEIDENTIFIER,
    @duration INT,
    @monday_start TIME, @monday_end TIME,
    @tuesday_start TIME, @tuesday_end TIME,
    @wednesday_start TIME, @wednesday_end TIME,
    @thursday_start TIME, @thursday_end TIME,
    @friday_start TIME, @friday_end TIME,
    @saturday_start TIME, @saturday_end TIME,
    @sunday_start TIME, @sunday_end TIME;

DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
SELECT
    practice_id, doctor_id, duration,
    monday_start, monday_end,
    tuesday_start, tuesday_end,
    wednesday_start, wednesday_end,
    thursday_start, thursday_end,
    friday_start, friday_end,
    saturday_start, saturday_end,
    sunday_start, sunday_end
FROM #DoctorsPractices;

OPEN cur;

FETCH NEXT FROM cur INTO
    @practice_id, @doctor_id, @duration,
    @monday_start, @monday_end,
    @tuesday_start, @tuesday_end,
    @wednesday_start, @wednesday_end,
    @thursday_start, @thursday_end,
    @friday_start, @friday_end,
    @saturday_start, @saturday_end,
    @sunday_start, @sunday_end;

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE @offset INT = -14;

    WHILE @offset <= 14
    BEGIN
        DECLARE @day DATE = DATEADD(DAY, @offset, CAST(GETDATE() AS DATE));
        DECLARE @weekday INT = ((DATEDIFF(DAY, '19000101', @day) + 1) % 7) + 1;

        DECLARE @start TIME = NULL;
        DECLARE @end TIME = NULL;

        IF @weekday = 1 BEGIN SET @start = @monday_start;    SET @end = @monday_end; END
        IF @weekday = 2 BEGIN SET @start = @tuesday_start;   SET @end = @tuesday_end; END
        IF @weekday = 3 BEGIN SET @start = @wednesday_start; SET @end = @wednesday_end; END
        IF @weekday = 4 BEGIN SET @start = @thursday_start;  SET @end = @thursday_end; END
        IF @weekday = 5 BEGIN SET @start = @friday_start;    SET @end = @friday_end; END
        IF @weekday = 6 BEGIN SET @start = @saturday_start;  SET @end = @saturday_end; END
        IF @weekday = 7 BEGIN SET @start = @sunday_start;    SET @end = @sunday_end; END

        IF @start IS NOT NULL AND @end IS NOT NULL
        BEGIN
            DECLARE @slot INT = 0;
            DECLARE @maxSlots INT = DATEDIFF(MINUTE, @start, @end) / @duration;

            WHILE @slot < @maxSlots
            BEGIN
                IF (@slot % 3 = 0)
                BEGIN
                    DECLARE @appointmentTime DATETIME2 =
                        DATEADD(
                            MINUTE,
                            @slot * @duration + DATEDIFF(MINUTE, CAST('00:00:00' AS TIME), @start),
                            CAST(@day AS DATETIME2)
                        );

                    DECLARE @patientIndex INT =
                        ((ABS(CHECKSUM(CONVERT(NVARCHAR(36), @doctor_id) + CONVERT(NVARCHAR(30), @appointmentTime, 126) + CONVERT(NVARCHAR(36), @practice_id))) % 20) + 1);

                    DECLARE @patient_id UNIQUEIDENTIFIER =
                        (SELECT patient_id FROM #PatientsSeed WHERE rn = @patientIndex);

                    DECLARE @status INT =
                        CASE
                            WHEN @offset < -7 THEN CASE @slot % 4 WHEN 0 THEN 1 WHEN 1 THEN 1 WHEN 2 THEN 2 ELSE 3 END
                            WHEN @offset < 0  THEN CASE @slot % 5 WHEN 0 THEN 1 WHEN 1 THEN 1 WHEN 2 THEN 0 WHEN 3 THEN 2 ELSE 3 END
                            ELSE 0
                        END;

                    IF NOT EXISTS
                    (
                        SELECT 1
                        FROM appointments a
                        WHERE a.practice_id = @practice_id
                          AND a.doctor_id = @doctor_id
                          AND a.patient_id = @patient_id
                          AND a.[time] = @appointmentTime
                    )
                    BEGIN
                        INSERT INTO appointments
                        (
                            id, practice_id, patient_id, doctor_id, [time], [Status]
                        )
                        VALUES
                        (
                            NEWID(), @practice_id, @patient_id, @doctor_id, @appointmentTime, @status
                        );
                    END
                END

                SET @slot += 1;
            END
        END

        SET @offset += 1;
    END

    FETCH NEXT FROM cur INTO
        @practice_id, @doctor_id, @duration,
        @monday_start, @monday_end,
        @tuesday_start, @tuesday_end,
        @wednesday_start, @wednesday_end,
        @thursday_start, @thursday_end,
        @friday_start, @friday_end,
        @saturday_start, @saturday_end,
        @sunday_start, @sunday_end;
END

CLOSE cur;
DEALLOCATE cur;

------------------------------------------------------------
-- 4) RESULT CHECKS
------------------------------------------------------------
SELECT COUNT(*) AS users_count FROM users;
SELECT COUNT(*) AS doctors_count FROM doctors;
SELECT COUNT(*) AS patients_count FROM patients;
SELECT COUNT(*) AS practices_count FROM practices;
SELECT COUNT(*) AS appointments_count FROM appointments;