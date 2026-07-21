-- Roles
INSERT INTO Roles (RoleName)
VALUES
('Admin'),
('HR'),
('Manager'),
('Employee');
-- ==========================

-- Departments
INSERT INTO Departments (DepartmentName, Description)
VALUES
('.NET', 'Development team working on .NET applications and services.'),
('Python', 'Development team responsible for Python-based applications and APIs.'),
('React', 'Frontend development team using React.js.'),
('Angular', 'Frontend development team using Angular framework.'),
('React Native', 'Mobile application development team using React Native.'),
('QA', 'Quality Assurance team responsible for testing and quality control.'),
('DevOps', 'Infrastructure, CI/CD, cloud deployment, and automation team.'),
('PHP', 'Development team responsible for PHP-based web applications.');
-- ==========================

-- Holidays - 2026
-- ==========================
INSERT INTO Holidays
(HolidayName, HolidayDate, Description, IsOptional)
VALUES
('Makar Sankranti', '2026-01-14', 'Harvest festival celebrated across India.', 0),
('Maha Shivratri', '2026-02-15', 'Festival dedicated to Lord Shiva.', 0),
('Holika Dahan', '2026-03-03', 'Bonfire marking the victory of good over evil.', 1),
('Holi (Dhulandi)', '2026-03-04', 'Festival of Colors.', 0),
('Ram Navami', '2026-03-27', 'Birth anniversary of Lord Rama.', 0),
('Hanuman Jayanti', '2026-04-02', 'Birth anniversary of Lord Hanuman.', 1),
('Akshaya Tritiya', '2026-04-20', 'Auspicious day for prosperity and new beginnings.', 1),
('Guru Purnima', '2026-07-29', 'Festival honoring spiritual and academic teachers.', 1),
('Raksha Bandhan', '2026-08-28', 'Celebration of the bond between brothers and sisters.', 1),
('Janmashtami', '2026-09-03', 'Birth anniversary of Lord Krishna.', 0),
('Ganesh Chaturthi', '2026-09-14', 'Birth of Lord Ganesha.', 0),
('Sharad Navratri Begins', '2026-10-11', 'Beginning of the nine-day festival dedicated to Goddess Durga.', 1),
('Durga Ashtami', '2026-10-18', 'Important day during Navratri.', 1),
('Dussehra (Vijayadashami)', '2026-10-20', 'Victory of Lord Rama over Ravana.', 0),
('Karva Chauth', '2026-10-29', 'Festival observed by married Hindu women.', 1),
('Dhanteras', '2026-11-08', 'Beginning of the Diwali festivities.', 1),
('Naraka Chaturdashi', '2026-11-09', 'Also known as Choti Diwali.', 1),
('Diwali (Lakshmi Puja)', '2026-11-10', 'Festival of Lights.', 0),
('Govardhan Puja', '2026-11-11', 'Celebration of Lord Krishna lifting Govardhan Hill.', 1),
('Bhai Dooj', '2026-11-12', 'Celebration of the bond between brothers and sisters.', 1);

-- Leave Types
-- ==========================
INSERT INTO LeaveType (LeaveTypeName, MaxDaysPerYear, IsPaid)
VALUES
('Sick Leave', 6, 1),
('Earned Leave (EL)', 12, 1),
('Festival/Flexible Leave (FFL)', 1, 1),
('Marriage Leave (ML)', 3, 1);