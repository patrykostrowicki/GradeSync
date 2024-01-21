CREATE DATABASE IF NOT EXISTS `dziennik` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `dziennik`;

-- Tabela uzytkownicy
CREATE TABLE IF NOT EXISTS `uzytkownicy` (
  `login` varchar(50) NOT NULL,
  `haslo` varchar(50) NOT NULL,
  `typ` varchar(50) NOT NULL,
  PRIMARY KEY (`login`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Tabela uczniowie
CREATE TABLE IF NOT EXISTS `uczniowie` (
  `login` varchar(50) NOT NULL,
  `imie_nazwisko` varchar(100) NOT NULL,
  `klasa` varchar(50) NOT NULL,
  `ocena_z_zachowania` decimal(2,1) DEFAULT NULL,
  PRIMARY KEY (`login`),
  FOREIGN KEY (`login`) REFERENCES `uzytkownicy` (`login`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Tabela nauczyciele
CREATE TABLE IF NOT EXISTS `nauczyciele` (
  `login` varchar(255) NOT NULL,
  `imie_nazwisko` varchar(255) DEFAULT NULL,
  `klasa` varchar(255) DEFAULT NULL,
  `inne` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`inne`)),
  PRIMARY KEY (`login`),
  FOREIGN KEY (`login`) REFERENCES `uzytkownicy` (`login`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Tabela frekwencja
CREATE TABLE IF NOT EXISTS `frekwencja` (
  `data` timestamp NOT NULL DEFAULT current_timestamp(),
  `przedmiot` varchar(100) DEFAULT NULL,
  `typ` int(11) DEFAULT NULL CHECK (`typ` >= 1 and `typ` <= 3),
  `login` varchar(50) NOT NULL,
  `uczen` varchar(50) DEFAULT NULL,
  `klasa` varchar(10) DEFAULT NULL,
  FOREIGN KEY (`login`) REFERENCES `uczniowie` (`login`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Tabela oceny
CREATE TABLE IF NOT EXISTS `oceny` (
  `uczen_login` varchar(50) NOT NULL,
  `wystawil` varchar(100) DEFAULT NULL,
  `ocena` decimal(2,1) DEFAULT NULL,
  `przedmiot` varchar(50) DEFAULT NULL,
  `data_wystawienia` date DEFAULT curdate(),
  `opis` text DEFAULT NULL,
  `klasa` varchar(10) DEFAULT NULL,
  `wystawil_login` varchar(55) NOT NULL,
  PRIMARY KEY (`uczen_login`, `wystawil_login`),
  FOREIGN KEY (`uczen_login`) REFERENCES `uczniowie` (`login`) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (`wystawil_login`) REFERENCES `nauczyciele` (`login`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Tabela uwagi_i_osiagniecia
CREATE TABLE IF NOT EXISTS `uwagi_i_osiagniecia` (
  `uczen` varchar(50) DEFAULT NULL,
  `uczen_login` varchar(50) NOT NULL,
  `wystawil` varchar(50) DEFAULT NULL,
  `wystawil_login` varchar(50) NOT NULL,
  `data` date DEFAULT NULL,
  `tresc` text DEFAULT NULL,
  `klasa` varchar(10) DEFAULT NULL,
  `typ` int(11) DEFAULT NULL CHECK (`typ` in (1,2)),
  PRIMARY KEY (`uczen_login`, `wystawil_login`),
  FOREIGN KEY (`uczen_login`) REFERENCES `uczniowie` (`login`) ON DELETE CASCADE ON UPDATE CASCADE,
  FOREIGN KEY (`wystawil_login`) REFERENCES `nauczyciele` (`login`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Tabela wydarzenia
CREATE TABLE IF NOT EXISTS `wydarzenia` (
  `klasa` varchar(10) NOT NULL,
  `wystawil` varchar(255) NOT NULL,
  `wystawil_login` varchar(50) NOT NULL,
  `data` date NOT NULL DEFAULT current_timestamp(),
  `termin` date NOT NULL,
  `przedmiot` varchar(255) NOT NULL,
  `opis` text NOT NULL,
  `typ` int(11) NOT NULL,
  CONSTRAINT `chk_typ` CHECK (`typ` between 1 and 5),
  FOREIGN KEY (`wystawil_login`) REFERENCES `nauczyciele` (`login`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Tabela plan_zajec
CREATE TABLE IF NOT EXISTS `plan_zajec` (
  `klasa` varchar(50) NOT NULL,
  `semestr` tinyint(4) DEFAULT NULL CHECK (`semestr` in (1,2)),
  `poniedzialek` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`poniedzialek`)),
  `wtorek` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`wtorek`)),
  `sroda` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`sroda`)),
  `czwartek` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`czwartek`)),
  `piatek` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`piatek`))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Przykładowe dane
INSERT INTO `plan_zajec` (`klasa`, `semestr`, `poniedzialek`, `wtorek`, `sroda`, `czwartek`, `piatek`) VALUES
	('3b', 1, '{"lek3": {"przedmiot": "Matematyka", "prowadzacy": "Maria Nowak", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 102"},\r\n  "lek4": {"przedmiot": "Historia", "prowadzacy": "Tomasz Nowicki", "prowadzacy_login": "tomek.historyk23", "sala": "Sala 105"},\r\n  "lek5": {"przedmiot": "Język Polski", "prowadzacy": "Janina Kowalczyk", "prowadzacy_login": "janka.polonistka54", "sala": "Sala 106"},\r\n  "lek6": {"przedmiot": "Wychowanie fizyczne", "prowadzacy": "Barbara Kowalska", "prowadzacy_login": "barbara.sportowa78", "sala": "Sala gimnastyczna"}}', '{"lek0": {"przedmiot": "Język Angielski", "prowadzacy": "Ewa Wisniewska", "prowadzacy_login": "ewa.english01", "sala": "Sala 107"},\r\n  "lek1": {"przedmiot": "Język Niemiecki", "prowadzacy": "Krzysztof Michalski", "prowadzacy_login": "krzysztof.deutsch45", "sala": "Sala 109"},\r\n  "lek2": {"przedmiot": "Biologia", "prowadzacy": "Anna Zalewska", "prowadzacy_login": "anna.bio90", "sala": "Sala 104"},\r\n  "lek3": {"przedmiot": "Chemia", "prowadzacy": "Jan Kowalski", "prowadzacy_login": "jan.chemik32", "sala": "Sala 103"}}', '{"lek4": {"przedmiot": "Geografia", "prowadzacy": "Magdalena Nowak", "prowadzacy_login": "magda.geo67", "sala": "Sala 108"},\r\n  "lek5": {"przedmiot": "Informatyka", "prowadzacy": "Piotr Kowalczyk", "prowadzacy_login": "piotr.itmaster99", "sala": "Sala 110"},\r\n  "lek6": {"przedmiot": "Edukacja do bezpieczeństwa", "prowadzacy": "Andrzej Dragan", "prowadzacy_login": "andrzej.safeedu88", "sala": "Sala 111"},\r\n  "lek7": {"przedmiot": "Podstawy przedsiębiorczości", "prowadzacy": "Maria Nowakowska", "prowadzacy_login": "maria.biznes101", "sala": "Sala 112"}}', '{"lek0": {"przedmiot": "Język Polski", "prowadzacy": "Janina Kowalczyk", "prowadzacy_login": "janka.polonistka54", "sala": "Sala 106"},\r\n  "lek1": {"przedmiot": "Historia", "prowadzacy": "Tomasz Nowicki", "prowadzacy_login": "tomek.historyk23", "sala": "Sala 105"},\r\n  "lek2": {"przedmiot": "Wychowanie fizyczne", "prowadzacy": "Barbara Kowalska", "prowadzacy_login": "barbara.sportowa78", "sala": "Sala gimnastyczna"},\r\n  "lek3": {"przedmiot": "Matematyka", "prowadzacy": "Maria Nowak", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 102"}}', '{"lek5": {"przedmiot": "Chemia", "prowadzacy": "Jan Kowalski", "prowadzacy_login": "jan.chemik32", "sala": "Sala 103"},\r\n  "lek6": {"przedmiot": "Fizyka", "prowadzacy": "Anna Zalewska", "prowadzacy_login": "anna.physicist22", "sala": "Sala 104"},\r\n  "lek7": {"przedmiot": "Język Angielski", "prowadzacy": "Ewa Wisniewska", "prowadzacy_login": "ewa.english01", "sala": "Sala 107"},\r\n  "lek8": {"przedmiot": "Język Niemiecki", "prowadzacy": "Krzysztof Michalski", "prowadzacy_login": "krzysztof.deutsch45", "sala": "Sala 109"}}'),
	('3b', 2, '{"lek1": {"przedmiot": "Biologia", "prowadzacy": "Anna Zalewska", "prowadzacy_login": "anna.bio90", "sala": "Sala 104"},\r\n  "lek2": {"przedmiot": "Chemia", "prowadzacy": "Jan Kowalski", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 103"},\r\n  "lek3": {"przedmiot": "Język Polski", "prowadzacy": "Janina Kowalczyk", "prowadzacy_login": "janka.polonistka54", "sala": "Sala 106"},\r\n  "lek4": {"przedmiot": "Historia", "prowadzacy": "Tomasz Nowicki", "prowadzacy_login": "tomek.historyk23", "sala": "Sala 105"}}', '{"lek5": {"przedmiot": "Wychowanie fizyczne", "prowadzacy": "Barbara Kowalska", "prowadzacy_login": "barbara.sportowa78", "sala": "Sala gimnastyczna"},\r\n  "lek6": {"przedmiot": "Język Angielski", "prowadzacy": "Ewa Wisniewska", "prowadzacy_login": "ewa.english01", "sala": "Sala 107"},\r\n  "lek7": {"przedmiot": "Język Niemiecki", "prowadzacy": "Krzysztof Michalski", "prowadzacy_login": "krzysztof.deutsch45", "sala": "Sala 109"},\r\n  "lek8": {"przedmiot": "Matematyka", "prowadzacy": "Maria Nowak", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 102"}}', '{"lek0": {"przedmiot": "Geografia", "prowadzacy": "Magdalena Nowak", "prowadzacy_login": "magda.geo67", "sala": "Sala 108"},\r\n  "lek1": {"przedmiot": "Informatyka", "prowadzacy": "Piotr Kowalczyk", "prowadzacy_login": "piotr.itmaster99", "sala": "Sala 110"},\r\n  "lek2": {"przedmiot": "Edukacja do bezpieczeństwa", "prowadzacy": "Andrzej Dragan", "prowadzacy_login": "andrzej.safeedu88", "sala": "Sala 111"},\r\n  "lek3": {"przedmiot": "Podstawy przedsiębiorczości", "prowadzacy": "Maria Nowakowska", "prowadzacy_login": "maria.biznes101", "sala": "Sala 112"}}', '{"lek4": {"przedmiot": "Język Polski", "prowadzacy": "Janina Kowalczyk", "prowadzacy_login": "janka.polonistka54", "sala": "Sala 106"},\r\n  "lek5": {"przedmiot": "Historia", "prowadzacy": "Tomasz Nowicki", "prowadzacy_login": "tomek.historyk23", "sala": "Sala 105"},\r\n  "lek6": {"przedmiot": "Wychowanie fizyczne", "prowadzacy": "Barbara Kowalska", "prowadzacy_login": "barbara.sportowa78", "sala": "Sala gimnastyczna"},\r\n  "lek7": {"przedmiot": "Matematyka", "prowadzacy": "Maria Nowak", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 102"}}', '{"lek0": {"przedmiot": "Chemia", "prowadzacy": "Jan Kowalski", "prowadzacy_login": "jan.chemik32", "sala": "Sala 103"},\r\n  "lek1": {"przedmiot": "Biologia", "prowadzacy": "Anna Zalewska", "prowadzacy_login": "anna.bio90", "sala": "Sala 104"},\r\n  "lek2": {"przedmiot": "Język Angielski", "prowadzacy": "Ewa Wisniewska", "prowadzacy_login": "ewa.english01", "sala": "Sala 107"},\r\n  "lek3": {"przedmiot": "Język Niemiecki", "prowadzacy": "Krzysztof Michalski", "prowadzacy_login": "krzysztof.deutsch45", "sala": "Sala 109"}}'),
	('4b', 2, '{"lek1": {"przedmiot": "Biologia", "prowadzacy": "Anna Zalewska", "prowadzacy_login": "anna.bio90", "sala": "Sala 104"},\r\n  "lek2": {"przedmiot": "Chemia", "prowadzacy": "Jan Kowalski", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 103"},\r\n  "lek3": {"przedmiot": "Język Polski", "prowadzacy": "Janina Kowalczyk", "prowadzacy_login": "janka.polonistka54", "sala": "Sala 106"},\r\n  "lek4": {"przedmiot": "Historia", "prowadzacy": "Tomasz Nowicki", "prowadzacy_login": "tomek.historyk23", "sala": "Sala 105"}}', '{"lek5": {"przedmiot": "Wychowanie fizyczne", "prowadzacy": "Barbara Kowalska", "prowadzacy_login": "barbara.sportowa78", "sala": "Sala gimnastyczna"},\r\n  "lek6": {"przedmiot": "Język Angielski", "prowadzacy": "Ewa Wisniewska", "prowadzacy_login": "ewa.english01", "sala": "Sala 107"},\r\n  "lek7": {"przedmiot": "Język Niemiecki", "prowadzacy": "Krzysztof Michalski", "prowadzacy_login": "krzysztof.deutsch45", "sala": "Sala 109"},\r\n  "lek8": {"przedmiot": "Matematyka", "prowadzacy": "Maria Nowak", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 102"}}', '{"lek0": {"przedmiot": "Geografia", "prowadzacy": "Magdalena Nowak", "prowadzacy_login": "magda.geo67", "sala": "Sala 108"},\r\n  "lek1": {"przedmiot": "Informatyka", "prowadzacy": "Piotr Kowalczyk", "prowadzacy_login": "piotr.itmaster99", "sala": "Sala 110"},\r\n  "lek2": {"przedmiot": "Edukacja do bezpieczeństwa", "prowadzacy": "Andrzej Dragan", "prowadzacy_login": "andrzej.safeedu88", "sala": "Sala 111"},\r\n  "lek3": {"przedmiot": "Podstawy przedsiębiorczości", "prowadzacy": "Maria Nowakowska", "prowadzacy_login": "maria.biznes101", "sala": "Sala 112"}}', '{"lek4": {"przedmiot": "Język Polski", "prowadzacy": "Janina Kowalczyk", "prowadzacy_login": "janka.polonistka54", "sala": "Sala 106"},\r\n  "lek5": {"przedmiot": "Historia", "prowadzacy": "Tomasz Nowicki", "prowadzacy_login": "tomek.historyk23", "sala": "Sala 105"},\r\n  "lek6": {"przedmiot": "Wychowanie fizyczne", "prowadzacy": "Barbara Kowalska", "prowadzacy_login": "barbara.sportowa78", "sala": "Sala gimnastyczna"},\r\n  "lek7": {"przedmiot": "Matematyka", "prowadzacy": "Maria Nowak", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 102"}}', '{"lek0": {"przedmiot": "Chemia", "prowadzacy": "Jan Kowalski", "prowadzacy_login": "jan.chemik32", "sala": "Sala 103"},\r\n  "lek1": {"przedmiot": "Biologia", "prowadzacy": "Anna Zalewska", "prowadzacy_login": "anna.bio90", "sala": "Sala 104"},\r\n  "lek2": {"przedmiot": "Język Angielski", "prowadzacy": "Ewa Wisniewska", "prowadzacy_login": "ewa.english01", "sala": "Sala 107"},\r\n  "lek3": {"przedmiot": "Język Niemiecki", "prowadzacy": "Krzysztof Michalski", "prowadzacy_login": "krzysztof.deutsch45", "sala": "Sala 109"}}');

