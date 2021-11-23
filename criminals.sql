-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Nov 23, 2021 at 11:13 PM
-- Server version: 10.4.21-MariaDB
-- PHP Version: 8.0.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `cryminals`
--

-- --------------------------------------------------------

--
-- Table structure for table `criminals`
--

CREATE TABLE `criminals` (
  `id` int(11) NOT NULL,
  `fk_owner_id` int(11) NOT NULL,
  `name` varchar(32) NOT NULL,
  `gender` enum('MALE','FEMALE','UNDEFINED','') NOT NULL,
  `avatar` varchar(256) NOT NULL,
  `rarity` enum('COMMON','RARE','EPIC','LEGENDARY') NOT NULL,
  `power` int(11) NOT NULL,
  `moneyRatio` int(11) NOT NULL,
  `health` int(11) NOT NULL,
  `stamina` int(11) NOT NULL,
  `job` varchar(64) NOT NULL,
  `alignment` varchar(64) NOT NULL,
  `status` enum('IDLING','WORKING','WOUNDED','DISEASED','WANTED','BUSTED','ARRESTED','DEAD','UNCONSCIOUS','DRUNK','DRUGGED','ADDICT') NOT NULL DEFAULT 'IDLING'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `criminals`
--

INSERT INTO `criminals` (`id`, `fk_owner_id`, `name`, `gender`, `avatar`, `rarity`, `power`, `moneyRatio`, `health`, `stamina`, `job`, `alignment`, `status`) VALUES
(133, 1, 'wip', 'MALE', 'none', 'COMMON', 323, 0, 111, 100, 'none', 'none', 'IDLING'),
(134, 1, 'wip', 'FEMALE', 'none', 'COMMON', 399, 0, 225, 94, 'none', 'none', 'IDLING'),
(135, 1, 'wip', 'UNDEFINED', 'none', 'RARE', 995, 0, 365, 125, 'none', 'none', 'IDLING'),
(136, 1, 'wip', 'FEMALE', 'none', 'RARE', 440, 0, 364, 157, 'none', 'none', 'IDLING'),
(137, 1, 'wip', 'UNDEFINED', 'none', 'COMMON', 233, 0, 153, 96, 'none', 'none', 'IDLING'),
(138, 1, 'wip', 'UNDEFINED', 'none', 'COMMON', 217, 0, 199, 95, 'none', 'none', 'IDLING');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `criminals`
--
ALTER TABLE `criminals`
  ADD PRIMARY KEY (`id`),
  ADD KEY `criminal_owner` (`fk_owner_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `criminals`
--
ALTER TABLE `criminals`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=139;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `criminals`
--
ALTER TABLE `criminals`
  ADD CONSTRAINT `criminal_owner` FOREIGN KEY (`fk_owner_id`) REFERENCES `accounts` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
