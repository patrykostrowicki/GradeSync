-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Wersja serwera:               10.4.28-MariaDB - mariadb.org binary distribution
-- Serwer OS:                    Win64
-- HeidiSQL Wersja:              12.6.0.6765
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Zrzut struktury bazy danych dziennik
CREATE DATABASE IF NOT EXISTS `dziennik` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `dziennik`;

-- Zrzut struktury tabela dziennik.frekwencja
CREATE TABLE IF NOT EXISTS `frekwencja` (
  `data` timestamp NOT NULL DEFAULT current_timestamp(),
  `przedmiot` varchar(100) DEFAULT NULL,
  `typ` int(11) DEFAULT NULL CHECK (`typ` >= 1 and `typ` <= 3),
  `login` varchar(50) DEFAULT NULL,
  `uczen` varchar(50) DEFAULT NULL,
  `klasa` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Zrzucanie danych dla tabeli dziennik.frekwencja: ~17 rows (około)
INSERT INTO `frekwencja` (`data`, `przedmiot`, `typ`, `login`, `uczen`, `klasa`) VALUES
	('2023-12-10 10:43:08', 'Fizyka', 1, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2023-12-17 13:34:35', 'Fizyka', 2, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2023-12-17 13:34:50', 'Fizyka', 3, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2023-12-15 23:00:00', 'Wychowanie fizyczne', 3, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2023-12-17 23:00:00', 'Język Polski', 1, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2023-12-18 23:00:00', 'Język Angielski', 2, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2023-12-19 23:00:00', 'Język Niemiecki', 3, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2023-12-20 23:00:00', 'Biologia', 1, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2023-12-22 23:00:00', 'Geografia', 3, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2023-12-24 23:00:00', 'Edukacja do bezpieczeństwa', 2, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2023-12-11 14:37:18', 'Biologia', 1, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2024-01-07 21:30:27', 'Chemia', 3, 'ala.makota', 'Ala Makota', '3b'),
	('2024-01-07 21:34:03', 'Chemia', 3, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2024-01-07 21:34:03', 'Chemia', 1, 'jan.nowak', 'Jan Nowak', '3b'),
	('2024-01-09 18:55:31', 'Chemia', 3, 'jan.kowalski21', 'Jan Kowalski', '3b'),
	('2024-01-09 18:55:31', 'Chemia', 1, 'jan.nowak', 'Jan Nowak', '3b'),
	('2024-01-09 19:06:50', 'Chemia', 3, 'ala.makota', 'Ala Makota', '3b');

-- Zrzut struktury tabela dziennik.nauczyciele
CREATE TABLE IF NOT EXISTS `nauczyciele` (
  `login` varchar(255) DEFAULT NULL,
  `imie_nazwisko` varchar(255) DEFAULT NULL,
  `klasa` varchar(255) DEFAULT NULL,
  `inne` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`inne`))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Zrzucanie danych dla tabeli dziennik.nauczyciele: ~1 rows (około)
INSERT INTO `nauczyciele` (`login`, `imie_nazwisko`, `klasa`, `inne`) VALUES
	('elo.melo', 'elo melo', '4b', '{"przedmioty":["Matematyka","Historia","Geografia","Podstawy przedsiębiorczości"]}');

-- Zrzut struktury tabela dziennik.oceny
CREATE TABLE IF NOT EXISTS `oceny` (
  `uczen_login` varchar(50) DEFAULT NULL,
  `wystawil` varchar(100) DEFAULT NULL,
  `ocena` decimal(2,1) DEFAULT NULL,
  `przedmiot` varchar(50) DEFAULT NULL,
  `data_wystawienia` date DEFAULT curdate(),
  `opis` text DEFAULT NULL,
  `klasa` varchar(10) DEFAULT NULL,
  `wystawil_login` varchar(55) DEFAULT NULL,
  KEY `uczen_login` (`uczen_login`),
  CONSTRAINT `oceny_ibfk_1` FOREIGN KEY (`uczen_login`) REFERENCES `uczniowie` (`login`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Zrzucanie danych dla tabeli dziennik.oceny: ~0 rows (około)

-- Zrzut struktury tabela dziennik.plan_zajec
CREATE TABLE IF NOT EXISTS `plan_zajec` (
  `klasa` varchar(50) NOT NULL,
  `semestr` tinyint(4) DEFAULT NULL CHECK (`semestr` in (1,2)),
  `poniedzialek` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`poniedzialek`)),
  `wtorek` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`wtorek`)),
  `sroda` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`sroda`)),
  `czwartek` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`czwartek`)),
  `piatek` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`piatek`))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Zrzucanie danych dla tabeli dziennik.plan_zajec: ~2 rows (około)
INSERT INTO `plan_zajec` (`klasa`, `semestr`, `poniedzialek`, `wtorek`, `sroda`, `czwartek`, `piatek`) VALUES
	('3b', 1, '{"lek3": {"przedmiot": "Matematyka", "prowadzacy": "Maria Nowak", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 102"},\r\n  "lek4": {"przedmiot": "Historia", "prowadzacy": "Tomasz Nowicki", "prowadzacy_login": "tomek.historyk23", "sala": "Sala 105"},\r\n  "lek5": {"przedmiot": "Język Polski", "prowadzacy": "Janina Kowalczyk", "prowadzacy_login": "janka.polonistka54", "sala": "Sala 106"},\r\n  "lek6": {"przedmiot": "Wychowanie fizyczne", "prowadzacy": "Barbara Kowalska", "prowadzacy_login": "barbara.sportowa78", "sala": "Sala gimnastyczna"}}', '{"lek0": {"przedmiot": "Język Angielski", "prowadzacy": "Ewa Wisniewska", "prowadzacy_login": "ewa.english01", "sala": "Sala 107"},\r\n  "lek1": {"przedmiot": "Język Niemiecki", "prowadzacy": "Krzysztof Michalski", "prowadzacy_login": "krzysztof.deutsch45", "sala": "Sala 109"},\r\n  "lek2": {"przedmiot": "Biologia", "prowadzacy": "Anna Zalewska", "prowadzacy_login": "anna.bio90", "sala": "Sala 104"},\r\n  "lek3": {"przedmiot": "Chemia", "prowadzacy": "Jan Kowalski", "prowadzacy_login": "jan.chemik32", "sala": "Sala 103"}}', '{"lek4": {"przedmiot": "Geografia", "prowadzacy": "Magdalena Nowak", "prowadzacy_login": "magda.geo67", "sala": "Sala 108"},\r\n  "lek5": {"przedmiot": "Informatyka", "prowadzacy": "Piotr Kowalczyk", "prowadzacy_login": "piotr.itmaster99", "sala": "Sala 110"},\r\n  "lek6": {"przedmiot": "Edukacja do bezpieczeństwa", "prowadzacy": "Andrzej Dragan", "prowadzacy_login": "andrzej.safeedu88", "sala": "Sala 111"},\r\n  "lek7": {"przedmiot": "Podstawy przedsiębiorczości", "prowadzacy": "Maria Nowakowska", "prowadzacy_login": "maria.biznes101", "sala": "Sala 112"}}', '{"lek0": {"przedmiot": "Język Polski", "prowadzacy": "Janina Kowalczyk", "prowadzacy_login": "janka.polonistka54", "sala": "Sala 106"},\r\n  "lek1": {"przedmiot": "Historia", "prowadzacy": "Tomasz Nowicki", "prowadzacy_login": "tomek.historyk23", "sala": "Sala 105"},\r\n  "lek2": {"przedmiot": "Wychowanie fizyczne", "prowadzacy": "Barbara Kowalska", "prowadzacy_login": "barbara.sportowa78", "sala": "Sala gimnastyczna"},\r\n  "lek3": {"przedmiot": "Matematyka", "prowadzacy": "Maria Nowak", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 102"}}', '{"lek5": {"przedmiot": "Chemia", "prowadzacy": "Jan Kowalski", "prowadzacy_login": "jan.chemik32", "sala": "Sala 103"},\r\n  "lek6": {"przedmiot": "Fizyka", "prowadzacy": "Anna Zalewska", "prowadzacy_login": "anna.physicist22", "sala": "Sala 104"},\r\n  "lek7": {"przedmiot": "Język Angielski", "prowadzacy": "Ewa Wisniewska", "prowadzacy_login": "ewa.english01", "sala": "Sala 107"},\r\n  "lek8": {"przedmiot": "Język Niemiecki", "prowadzacy": "Krzysztof Michalski", "prowadzacy_login": "krzysztof.deutsch45", "sala": "Sala 109"}}'),
	('3b', 2, '{"lek1": {"przedmiot": "Biologia", "prowadzacy": "Anna Zalewska", "prowadzacy_login": "anna.bio90", "sala": "Sala 104"},\r\n  "lek2": {"przedmiot": "Chemia", "prowadzacy": "Jan Kowalski", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 103"},\r\n  "lek3": {"przedmiot": "Język Polski", "prowadzacy": "Janina Kowalczyk", "prowadzacy_login": "janka.polonistka54", "sala": "Sala 106"},\r\n  "lek4": {"przedmiot": "Historia", "prowadzacy": "Tomasz Nowicki", "prowadzacy_login": "tomek.historyk23", "sala": "Sala 105"}}', '{"lek5": {"przedmiot": "Wychowanie fizyczne", "prowadzacy": "Barbara Kowalska", "prowadzacy_login": "barbara.sportowa78", "sala": "Sala gimnastyczna"},\r\n  "lek6": {"przedmiot": "Język Angielski", "prowadzacy": "Ewa Wisniewska", "prowadzacy_login": "ewa.english01", "sala": "Sala 107"},\r\n  "lek7": {"przedmiot": "Język Niemiecki", "prowadzacy": "Krzysztof Michalski", "prowadzacy_login": "krzysztof.deutsch45", "sala": "Sala 109"},\r\n  "lek8": {"przedmiot": "Matematyka", "prowadzacy": "Maria Nowak", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 102"}}', '{"lek0": {"przedmiot": "Geografia", "prowadzacy": "Magdalena Nowak", "prowadzacy_login": "magda.geo67", "sala": "Sala 108"},\r\n  "lek1": {"przedmiot": "Informatyka", "prowadzacy": "Piotr Kowalczyk", "prowadzacy_login": "piotr.itmaster99", "sala": "Sala 110"},\r\n  "lek2": {"przedmiot": "Edukacja do bezpieczeństwa", "prowadzacy": "Andrzej Dragan", "prowadzacy_login": "andrzej.safeedu88", "sala": "Sala 111"},\r\n  "lek3": {"przedmiot": "Podstawy przedsiębiorczości", "prowadzacy": "Maria Nowakowska", "prowadzacy_login": "maria.biznes101", "sala": "Sala 112"}}', '{"lek4": {"przedmiot": "Język Polski", "prowadzacy": "Janina Kowalczyk", "prowadzacy_login": "janka.polonistka54", "sala": "Sala 106"},\r\n  "lek5": {"przedmiot": "Historia", "prowadzacy": "Tomasz Nowicki", "prowadzacy_login": "tomek.historyk23", "sala": "Sala 105"},\r\n  "lek6": {"przedmiot": "Wychowanie fizyczne", "prowadzacy": "Barbara Kowalska", "prowadzacy_login": "barbara.sportowa78", "sala": "Sala gimnastyczna"},\r\n  "lek7": {"przedmiot": "Matematyka", "prowadzacy": "Maria Nowak", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 102"}}', '{"lek0": {"przedmiot": "Chemia", "prowadzacy": "Jan Kowalski", "prowadzacy_login": "jan.chemik32", "sala": "Sala 103"},\r\n  "lek1": {"przedmiot": "Biologia", "prowadzacy": "Anna Zalewska", "prowadzacy_login": "anna.bio90", "sala": "Sala 104"},\r\n  "lek2": {"przedmiot": "Język Angielski", "prowadzacy": "Ewa Wisniewska", "prowadzacy_login": "ewa.english01", "sala": "Sala 107"},\r\n  "lek3": {"przedmiot": "Język Niemiecki", "prowadzacy": "Krzysztof Michalski", "prowadzacy_login": "krzysztof.deutsch45", "sala": "Sala 109"}}'),
	('4b', 2, '{"lek1": {"przedmiot": "Biologia", "prowadzacy": "Anna Zalewska", "prowadzacy_login": "anna.bio90", "sala": "Sala 104"},\r\n  "lek2": {"przedmiot": "Chemia", "prowadzacy": "Jan Kowalski", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 103"},\r\n  "lek3": {"przedmiot": "Język Polski", "prowadzacy": "Janina Kowalczyk", "prowadzacy_login": "janka.polonistka54", "sala": "Sala 106"},\r\n  "lek4": {"przedmiot": "Historia", "prowadzacy": "Tomasz Nowicki", "prowadzacy_login": "tomek.historyk23", "sala": "Sala 105"}}', '{"lek5": {"przedmiot": "Wychowanie fizyczne", "prowadzacy": "Barbara Kowalska", "prowadzacy_login": "barbara.sportowa78", "sala": "Sala gimnastyczna"},\r\n  "lek6": {"przedmiot": "Język Angielski", "prowadzacy": "Ewa Wisniewska", "prowadzacy_login": "ewa.english01", "sala": "Sala 107"},\r\n  "lek7": {"przedmiot": "Język Niemiecki", "prowadzacy": "Krzysztof Michalski", "prowadzacy_login": "krzysztof.deutsch45", "sala": "Sala 109"},\r\n  "lek8": {"przedmiot": "Matematyka", "prowadzacy": "Maria Nowak", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 102"}}', '{"lek0": {"przedmiot": "Geografia", "prowadzacy": "Magdalena Nowak", "prowadzacy_login": "magda.geo67", "sala": "Sala 108"},\r\n  "lek1": {"przedmiot": "Informatyka", "prowadzacy": "Piotr Kowalczyk", "prowadzacy_login": "piotr.itmaster99", "sala": "Sala 110"},\r\n  "lek2": {"przedmiot": "Edukacja do bezpieczeństwa", "prowadzacy": "Andrzej Dragan", "prowadzacy_login": "andrzej.safeedu88", "sala": "Sala 111"},\r\n  "lek3": {"przedmiot": "Podstawy przedsiębiorczości", "prowadzacy": "Maria Nowakowska", "prowadzacy_login": "maria.biznes101", "sala": "Sala 112"}}', '{"lek4": {"przedmiot": "Język Polski", "prowadzacy": "Janina Kowalczyk", "prowadzacy_login": "janka.polonistka54", "sala": "Sala 106"},\r\n  "lek5": {"przedmiot": "Historia", "prowadzacy": "Tomasz Nowicki", "prowadzacy_login": "tomek.historyk23", "sala": "Sala 105"},\r\n  "lek6": {"przedmiot": "Wychowanie fizyczne", "prowadzacy": "Barbara Kowalska", "prowadzacy_login": "barbara.sportowa78", "sala": "Sala gimnastyczna"},\r\n  "lek7": {"przedmiot": "Matematyka", "prowadzacy": "Maria Nowak", "prowadzacy_login": "nau.czyciel96", "sala": "Sala 102"}}', '{"lek0": {"przedmiot": "Chemia", "prowadzacy": "Jan Kowalski", "prowadzacy_login": "jan.chemik32", "sala": "Sala 103"},\r\n  "lek1": {"przedmiot": "Biologia", "prowadzacy": "Anna Zalewska", "prowadzacy_login": "anna.bio90", "sala": "Sala 104"},\r\n  "lek2": {"przedmiot": "Język Angielski", "prowadzacy": "Ewa Wisniewska", "prowadzacy_login": "ewa.english01", "sala": "Sala 107"},\r\n  "lek3": {"przedmiot": "Język Niemiecki", "prowadzacy": "Krzysztof Michalski", "prowadzacy_login": "krzysztof.deutsch45", "sala": "Sala 109"}}');

-- Zrzut struktury tabela dziennik.uczniowie
CREATE TABLE IF NOT EXISTS `uczniowie` (
  `login` varchar(50) DEFAULT NULL,
  `imie_nazwisko` varchar(100) NOT NULL,
  `klasa` varchar(50) NOT NULL,
  `ocena_z_zachowania` decimal(2,1) DEFAULT NULL,
  KEY `login` (`login`),
  CONSTRAINT `uczniowie_ibfk_1` FOREIGN KEY (`login`) REFERENCES `uzytkownicy` (`login`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Zrzucanie danych dla tabeli dziennik.uczniowie: ~2 rows (około)
INSERT INTO `uczniowie` (`login`, `imie_nazwisko`, `klasa`, `ocena_z_zachowania`) VALUES
	('ala.makota', 'Ala Makota', '3b', 2.0),
	('jan.kowalski21', 'Jan Kowalskii', '3b', 6.0);

-- Zrzut struktury tabela dziennik.uwagi_i_osiagniecia
CREATE TABLE IF NOT EXISTS `uwagi_i_osiagniecia` (
  `uczen` varchar(50) DEFAULT NULL,
  `uczen_login` varchar(50) DEFAULT NULL,
  `wystawil` varchar(50) DEFAULT NULL,
  `wystawil_login` varchar(50) DEFAULT NULL,
  `data` date DEFAULT NULL,
  `tresc` text DEFAULT NULL,
  `klasa` varchar(10) DEFAULT NULL,
  `typ` int(11) DEFAULT NULL CHECK (`typ` in (1,2)),
  KEY `uczen_login` (`uczen_login`),
  CONSTRAINT `uwagi_i_osiagniecia_ibfk_1` FOREIGN KEY (`uczen_login`) REFERENCES `uczniowie` (`login`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Zrzucanie danych dla tabeli dziennik.uwagi_i_osiagniecia: ~0 rows (około)

-- Zrzut struktury tabela dziennik.uzytkownicy
CREATE TABLE IF NOT EXISTS `uzytkownicy` (
  `login` varchar(50) NOT NULL,
  `haslo` varchar(50) NOT NULL,
  `typ` varchar(50) NOT NULL,
  PRIMARY KEY (`login`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Zrzucanie danych dla tabeli dziennik.uzytkownicy: ~4 rows (około)
INSERT INTO `uzytkownicy` (`login`, `haslo`, `typ`) VALUES
	('admin', 'admin', 'admin'),
	('ala.makota', 'test', 'uczen'),
	('elo.melo', 'test', 'nauczyciel'),
	('jan.kowalski21', 'test1234', 'uczen');

-- Zrzut struktury tabela dziennik.wydarzenia
CREATE TABLE IF NOT EXISTS `wydarzenia` (
  `klasa` varchar(10) NOT NULL,
  `wystawil` varchar(255) NOT NULL,
  `wystawil_login` varchar(50) NOT NULL,
  `data` date NOT NULL DEFAULT current_timestamp(),
  `termin` date NOT NULL,
  `przedmiot` varchar(255) NOT NULL,
  `opis` text NOT NULL,
  `typ` int(11) NOT NULL,
  CONSTRAINT `chk_typ` CHECK (`typ` between 1 and 5)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Zrzucanie danych dla tabeli dziennik.wydarzenia: ~39 rows (około)
INSERT INTO `wydarzenia` (`klasa`, `wystawil`, `wystawil_login`, `data`, `termin`, `przedmiot`, `opis`, `typ`) VALUES
	('3b', 'Nauczyciel Nowak', '', '2023-12-23', '2023-12-25', 'Biologia', 'układ pokarmowy rozwielitki', 2),
	('3b', 'Naucyzciel Kowalski', '', '2023-12-23', '2023-12-23', 'Matematyka', 'całki', 1),
	('3b', 'Nauczyciel Kowal', '', '2023-12-24', '2024-01-04', 'Chemia', 'reakcje redoks', 1),
	('3b', 'Nauczyciel Maj', '', '2023-12-25', '2024-01-08', 'Geografia', 'struktury geologiczne', 1),
	('3b', 'Nauczyciel Białek', '', '2023-12-26', '2024-01-10', 'WOS', 'prawa obywatelskie', 1),
	('3b', 'Nauczyciel Szymański', '', '2023-12-27', '2024-01-12', 'Informatyka', 'sieci komputerowe', 1),
	('3b', 'Nauczyciel Jankowski', '', '2023-12-24', '2023-12-30', 'Język angielski', 'czas przeszły', 2),
	('3b', 'Nauczyciel Kaczmarek', '', '2023-12-27', '2024-01-09', 'Historia', 'II wojna światowa', 2),
	('3b', 'Nauczyciel Sobczak', '', '2023-12-28', '2024-01-15', 'Fizyka', 'elektryczność i magnetyzm', 1),
	('3b', 'Nauczyciel Lewandowski', '', '2023-12-29', '2024-01-17', 'Chemia', 'kwasy i zasady', 1),
	('3b', 'Nauczyciel Wiśniewski', '', '2023-12-30', '2024-01-19', 'Biologia', 'fotosynteza', 1),
	('3b', 'Nauczyciel Kamiński', '', '2023-12-29', '2024-01-16', 'Język polski', 'barok w literaturze', 2),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-05-17', 'Matematyka', 'Opis zadania domowego', 3),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-12-12', 'Wychowanie fizyczne', 'Opis zadania domowego', 3),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-03-18', 'Historia', 'Opis zadania domowego', 3),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-09-27', 'Fizyka', 'Opis zadania domowego', 3),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-04-21', 'Język Angielski', 'Opis zadania domowego', 3),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-07-03', 'Biologia', 'Opis zadania domowego', 3),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2024-03-21', 'Edukacja do bezpieczeństwa', 'Opis zadania domowegoxcxzca', 4),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2023-07-19', 'Wychowanie fizyczne', 'Opis projektu', 4),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2023-06-04', 'Historia', 'Opis projektu', 4),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2023-04-02', 'Fizyka', 'Opis projektu', 4),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2023-06-15', 'Język Polski', 'Opis projektu', 4),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2023-10-27', 'Język Angielski', 'Opis projektu', 4),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2023-03-06', 'Biologia', 'Opis projektu', 4),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2023-11-01', 'Chemia', 'Opis projektu', 4),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2023-07-13', 'Geografia', 'Opis projektu', 4),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2023-07-20', 'Informatyka', 'Opis projektu', 4),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2023-09-08', 'Edukacja do bezpieczeństwa', 'Opis projektu', 4),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2023-05-02', 'Podstawy przedsiębiorczości', 'Opis projektu', 4),
	('3b', 'Teacher Name', 'nau.czyciel96', '2023-12-23', '2023-02-05', '', 'Opis ogłoszenia szkolnego', 5),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-08-09', '', 'Opis ogłoszenia szkolnego', 5),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-11-25', '', 'Opis ogłoszenia szkolnego', 5),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-02-02', '', 'Opis ogłoszenia szkolnego', 5),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-05-28', '', 'Opis ogłoszenia szkolnego', 5),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-11-25', '', 'Opis ogłoszenia szkolnego', 5),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-11-15', '', 'Opis ogłoszenia szkolnego', 5),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-10-24', '', 'Opis ogłoszenia szkolnego', 5),
	('3b', 'Teacher Name', '', '2023-12-23', '2023-10-08', '', 'Opis ogłoszenia szkolnego', 5);

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
