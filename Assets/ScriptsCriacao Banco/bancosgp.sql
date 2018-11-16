-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: 21-Out-2018 às 22:20
-- Versão do servidor: 10.1.36-MariaDB
-- versão do PHP: 7.2.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bancosgp`
--

-- --------------------------------------------------------

--
-- Estrutura da tabela `alternativa`
--

CREATE TABLE `alternativa` (
  `ID` bigint(20) NOT NULL,
  `CORRETO` tinyint(1) DEFAULT '0',
  `DESCRICAO` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `alternativa`
--

INSERT INTO `alternativa` (`ID`, `CORRETO`, `DESCRICAO`) VALUES
(1, 0, 'RESPOSTA 1'),
(2, 0, 'RESPOSTA 2'),
(3, 1, 'RESPOSTA CERTA'),
(4, 0, 'RESPOSTA 4'),
(5, 0, 'RESPOSTA 1'),
(6, 0, 'RESPOSTA 2'),
(7, 0, 'RESPOSTA 3'),
(8, 1, 'RESPOSTA CERTA'),
(9, 0, 'RESPOSTA 1'),
(10, 1, 'RESPOSTA CERTA'),
(11, 0, 'RESPOSTA 3'),
(12, 0, 'RESPOSTA 4');

-- --------------------------------------------------------

--
-- Estrutura da tabela `disciplina`
--

CREATE TABLE `disciplina` (
  `ID` bigint(20) NOT NULL,
  `DESCRICAO` varchar(255) DEFAULT NULL,
  `HABILITAR` tinyint(1) DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `disciplina`
--

INSERT INTO `disciplina` (`ID`, `DESCRICAO`, `HABILITAR`) VALUES
(1, 'CONHECIMENTOS GERAIS', 1);

-- --------------------------------------------------------

--
-- Estrutura da tabela `pergunta`
--

CREATE TABLE `pergunta` (
  `ID` bigint(20) NOT NULL,
  `DESCRICAO` varchar(255) DEFAULT NULL,
  `DIFICULDADE` varchar(255) DEFAULT NULL,
  `HABILITAR` tinyint(1) DEFAULT '0',
  `TAGS` longblob,
  `TEMPO` int(11) DEFAULT NULL,
  `DISCIPLINA_ID` bigint(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `pergunta`
--

INSERT INTO `pergunta` (`ID`, `DESCRICAO`, `DIFICULDADE`, `HABILITAR`, `TAGS`, `TEMPO`, `DISCIPLINA_ID`) VALUES
(1, 'PERGUNTA DICIL', 'DIFICIL', 1, 0xaced0005737200136a6176612e7574696c2e41727261794c6973747881d21d99c7619d03000149000473697a6578700000000077040000000078, 30, 1),
(2, 'PERGUNTA FACIL', 'FACIL', 1, 0xaced0005737200136a6176612e7574696c2e41727261794c6973747881d21d99c7619d03000149000473697a6578700000000077040000000078, 50, 1),
(3, 'PERGUNTA MEDIA', 'MEDIO', 1, 0xaced0005737200136a6176612e7574696c2e41727261794c6973747881d21d99c7619d03000149000473697a6578700000000077040000000078, 60, 1);

-- --------------------------------------------------------

--
-- Estrutura da tabela `pergunta_alternativa`
--

CREATE TABLE `pergunta_alternativa` (
  `Pergunta_ID` bigint(20) NOT NULL,
  `alternativas_ID` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `pergunta_alternativa`
--

INSERT INTO `pergunta_alternativa` (`Pergunta_ID`, `alternativas_ID`) VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 4),
(2, 5),
(2, 6),
(2, 7),
(2, 8),
(3, 9),
(3, 10),
(3, 11),
(3, 12);

-- --------------------------------------------------------

--
-- Estrutura da tabela `sala`
--

CREATE TABLE `sala` (
  `ID` bigint(20) NOT NULL,
  `DESCRICAO` varchar(255) DEFAULT NULL,
  `HABILITAR` tinyint(1) DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `sala`
--

INSERT INTO `sala` (`ID`, `DESCRICAO`, `HABILITAR`) VALUES
(1, 'SALA 1', 1);

-- --------------------------------------------------------

--
-- Estrutura da tabela `sala_pergunta`
--

CREATE TABLE `sala_pergunta` (
  `Sala_ID` bigint(20) NOT NULL,
  `pergunta_ID` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `sala_pergunta`
--

INSERT INTO `sala_pergunta` (`Sala_ID`, `pergunta_ID`) VALUES
(1, 1),
(1, 2),
(1, 3);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `alternativa`
--
ALTER TABLE `alternativa`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `disciplina`
--
ALTER TABLE `disciplina`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `pergunta`
--
ALTER TABLE `pergunta`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `FK_PERGUNTA_DISCIPLINA_ID` (`DISCIPLINA_ID`);

--
-- Indexes for table `pergunta_alternativa`
--
ALTER TABLE `pergunta_alternativa`
  ADD PRIMARY KEY (`Pergunta_ID`,`alternativas_ID`),
  ADD KEY `FK_PERGUNTA_ALTERNATIVA_alternativas_ID` (`alternativas_ID`);

--
-- Indexes for table `sala`
--
ALTER TABLE `sala`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `sala_pergunta`
--
ALTER TABLE `sala_pergunta`
  ADD PRIMARY KEY (`Sala_ID`,`pergunta_ID`),
  ADD KEY `FK_SALA_PERGUNTA_pergunta_ID` (`pergunta_ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `alternativa`
--
ALTER TABLE `alternativa`
  MODIFY `ID` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `disciplina`
--
ALTER TABLE `disciplina`
  MODIFY `ID` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `pergunta`
--
ALTER TABLE `pergunta`
  MODIFY `ID` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `sala`
--
ALTER TABLE `sala`
  MODIFY `ID` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Constraints for dumped tables
--

--
-- Limitadores para a tabela `pergunta`
--
ALTER TABLE `pergunta`
  ADD CONSTRAINT `FK_PERGUNTA_DISCIPLINA_ID` FOREIGN KEY (`DISCIPLINA_ID`) REFERENCES `disciplina` (`ID`);

--
-- Limitadores para a tabela `pergunta_alternativa`
--
ALTER TABLE `pergunta_alternativa`
  ADD CONSTRAINT `FK_PERGUNTA_ALTERNATIVA_Pergunta_ID` FOREIGN KEY (`Pergunta_ID`) REFERENCES `pergunta` (`ID`),
  ADD CONSTRAINT `FK_PERGUNTA_ALTERNATIVA_alternativas_ID` FOREIGN KEY (`alternativas_ID`) REFERENCES `alternativa` (`ID`);

--
-- Limitadores para a tabela `sala_pergunta`
--
ALTER TABLE `sala_pergunta`
  ADD CONSTRAINT `FK_SALA_PERGUNTA_Sala_ID` FOREIGN KEY (`Sala_ID`) REFERENCES `sala` (`ID`),
  ADD CONSTRAINT `FK_SALA_PERGUNTA_pergunta_ID` FOREIGN KEY (`pergunta_ID`) REFERENCES `pergunta` (`ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
