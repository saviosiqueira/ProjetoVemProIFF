-- phpMyAdmin SQL Dump
-- version 4.8.3
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: 23-Nov-2018 às 17:00
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
(1, 0, 'Eles afirmam: há mais pescadores do que peixe.'),
(2, 1, 'Eles afirmam que devem haver mais pescadores do que peixe.'),
(3, 0, 'Eles afirmam: existe mais pescadores do que peixe'),
(4, 0, 'Eles afirmam que deve existir mais pescadores do que peixe'),
(5, 0, 'objeto direto; adjunto adverbial'),
(6, 0, 'objeto direto; objeto indireto'),
(7, 1, 'objeto indireto; objeto indireto'),
(8, 0, ' adjunto adnominal; objeto direto'),
(9, 1, 'houve for'),
(10, 0, 'houve forem'),
(11, 0, 'houveram for'),
(12, 0, 'houveram forem'),
(13, 0, 'Até a pesquisa mais brilhante está condicionada às ferramentas que se dispõe em cada momento.'),
(14, 0, 'O primeiro avanço tecnológico é a cápsula pressurizada atrelada a um balão de hélio a que ele será l'),
(15, 0, 'Extremamente triste com a condição que se encontra, ele passa a se culpar [...]'),
(16, 1, 'De repente, é a solução que todos gostariam.'),
(17, 0, 'Eles lhe haviam contado que o Congresso é uma mamata.'),
(18, 1, 'Não ficarão órfãs porque deixei-as já adultas.'),
(19, 0, 'Quando transferiu-se para Maceió, tudo foi resolvido.'),
(20, 0, 'Havia formado-se em Engenharia, mas não exercia a profissão.');

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
(1, 'Português', 1);

-- --------------------------------------------------------

--
-- Estrutura da tabela `pergunta`
--

CREATE TABLE `pergunta` (
  `ID` bigint(20) NOT NULL,
  `DESCRICAO` longtext,
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
(1, 'Um dos complexos estuarinos mais importantes do país \nestá morrendo. Em parte das lagoas já não se encontram mais as grandes e suculentas ostras, os siris tradicionais por seu coral e também o caranguejo. Sem falar na ausência que mais atinge os pescadores, a dos próprios peixes. Eles afirmam que existem mais pescadores do que peixe para ser pescado. \nNo trecho da Mundaú, que vai do cais da lancha no dique estrada até ao papódromo, não existe quase mais vida lacunar. É só lixo, e lixo, e lixo. E os peixes que ainda sobrevivem não são bons para o consumo  (O Jornal, 28/02/2010).\n\nConsiderando-se aspectos da norma-padrão da língua portuguesa, a oração Eles afirmam que existem mais pescadores d', 'DIFICIL', 1, 0xaced0005737200136a6176612e7574696c2e41727261794c6973747881d21d99c7619d03000149000473697a65787000000001770400000001740009706f7274756775657378, 90, 1),
(2, 'Já tive muitas capas e infinitos guarda-chuvas, mas acabei me cansando de tê-los e perdê-los; há anos vivo sem nenhum desses abrigos, e também como toda gente, sem chapéu \n(Rubem Braga).\n\ntê-los e perdê-los, disse o autor. Os pronomes nesse trecho são, respectivamente,', 'DIFICIL', 1, 0xaced0005737200136a6176612e7574696c2e41727261794c6973747881d21d99c7619d03000149000473697a6578700000000077040000000078, 103, 1),
(3, 'Assinale a alternativa que completa correta e respectivamente as lacunas:\n\nI. Não __________ meios de convencê-lo a ficar mais.\n\nII. Sairemos quando _______ dez horas.', 'MEDIO', 1, 0xaced0005737200136a6176612e7574696c2e41727261794c6973747881d21d99c7619d03000149000473697a6578700000000077040000000078, 60, 1),
(4, 'Em qual opção o conetivo foi empregado adequadamente, de acordo com a norma-padrão da língua portuguesa?', 'FACIL', 1, 0xaced0005737200136a6176612e7574696c2e41727261794c6973747881d21d99c7619d03000149000473697a6578700000000077040000000078, 30, 1),
(5, 'Em qual opção o pronome oblíquo não viola a norma-padrão da língua portuguesa?', 'DIFICIL', 1, 0xaced0005737200136a6176612e7574696c2e41727261794c6973747881d21d99c7619d03000149000473697a6578700000000077040000000078, 60, 1);

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
(3, 12),
(4, 13),
(4, 14),
(4, 15),
(4, 16),
(5, 17),
(5, 18),
(5, 19),
(5, 20);

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
(1, 'Colégio Luiz Ferraz', 1);

-- --------------------------------------------------------

--
-- Estrutura da tabela `sala_pergunta`
--

CREATE TABLE `sala_pergunta` (
  `Sala_ID` bigint(20) NOT NULL,
  `perguntas_ID` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Extraindo dados da tabela `sala_pergunta`
--

INSERT INTO `sala_pergunta` (`Sala_ID`, `perguntas_ID`) VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 4),
(1, 5);

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
  ADD PRIMARY KEY (`Sala_ID`,`perguntas_ID`),
  ADD KEY `FK_SALA_PERGUNTA_perguntas_ID` (`perguntas_ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `alternativa`
--
ALTER TABLE `alternativa`
  MODIFY `ID` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `disciplina`
--
ALTER TABLE `disciplina`
  MODIFY `ID` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `pergunta`
--
ALTER TABLE `pergunta`
  MODIFY `ID` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

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
  ADD CONSTRAINT `FK_SALA_PERGUNTA_perguntas_ID` FOREIGN KEY (`perguntas_ID`) REFERENCES `pergunta` (`ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
