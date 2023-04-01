DELETE FROM `Unit`;
DELETE FROM `Feat`;
DELETE FROM `Role`;
DELETE FROM `RoleType`;
DELETE FROM `GuildUnitModifier`;
DELETE FROM `GuildRoleModifier`;
DELETE FROM `Guild`;
DELETE FROM `User`;
DELETE FROM `GuildFeat`;

-- Insert User
INSERT INTO user (`ID`, `Email`, `Salt`, `Hash`, `TimeCreated`)
VALUES (
	'6c833097-d117-4215-888e-8f4d882af274', 
    'user@example.com', 
    x'5D1A88A3E921ADEB6804AB357EE18306EB50D3355CFC56E9624207A57B75263BD384101BE536CDFDD12D5456829F78CF295B6F5EDF939539DD35D0C4FA16CCEC', 
    x'1C935180347A655C40B6678BCC66828E15DE6F503200A27C710CFA0EA5080371033226A9F963C0B3268F74C73383236059E6F8510595EDEBE9B3A3DDCAE52B10',
    UTC_TIMESTAMP()
);

-- Insert Guild
INSERT INTO guild (`ID`, `UserID`, `Name`, `Currency`, `TimeCreated`, `TimeUnitRefresh`, `TimeMissionRefresh`)
VALUES (
	'33c8fcaa-302c-4d4e-9953-c0bb5839f3f5', 
    '6c833097-d117-4215-888e-8f4d882af274',
    'Test', 
    100,
    UTC_TIMESTAMP(),
    UTC_TIMESTAMP(),
    UTC_TIMESTAMP()
);

-- Insert RoleType
INSERT INTO `RoleType` (`ID`, `Name`, `TimeCreated`) VALUES ('6f4c1af6-5fa4-4b93-a0b7-38908a58eea6', 'Affinity', UTC_TIMESTAMP());
INSERT INTO `RoleType` (`ID`, `Name`, `TimeCreated`) VALUES ('7a2b3b75-05c9-446c-a168-230eb4672b00', 'Profession', UTC_TIMESTAMP());

-- Insert Role
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('d751bb0c-e305-421e-adb6-23010cb5c8ae', 'Spellblade', '6f4c1af6-5fa4-4b93-a0b7-38908a58eea6', UTC_TIMESTAMP());
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('7d39e218-7de4-4f4d-9180-b4e2b66519e0', 'Paladin', '6f4c1af6-5fa4-4b93-a0b7-38908a58eea6', UTC_TIMESTAMP());
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('950dc1c9-15c8-41b1-8d29-53366a5bb9f3', 'Scoundrel', '6f4c1af6-5fa4-4b93-a0b7-38908a58eea6', UTC_TIMESTAMP());
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('8835f33d-d949-4d26-8fff-fa4601797c67', 'Dark Knight', '6f4c1af6-5fa4-4b93-a0b7-38908a58eea6', UTC_TIMESTAMP());
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('e6364aa8-088f-4bc2-bb0f-2a7a8c929306', 'Knight', '6f4c1af6-5fa4-4b93-a0b7-38908a58eea6', UTC_TIMESTAMP());
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('e878bde4-fc00-4af5-9e3b-580f3fec32d4', 'Lancer', '6f4c1af6-5fa4-4b93-a0b7-38908a58eea6', UTC_TIMESTAMP());
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('7ba42cc4-0861-47ba-90ce-ea7024486a2c', 'Ranger', '6f4c1af6-5fa4-4b93-a0b7-38908a58eea6', UTC_TIMESTAMP());
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('b41294bf-0e32-41ea-b5ea-86c4a2b5a39f', 'Marksman', '6f4c1af6-5fa4-4b93-a0b7-38908a58eea6', UTC_TIMESTAMP());

INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('99463e3a-3a3a-40a9-991a-0144ad9070a1', 'Miner', '7a2b3b75-05c9-446c-a168-230eb4672b00', UTC_TIMESTAMP());
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('43c6b1f2-d014-4332-9598-02b99d901d2b', 'Furrier', '7a2b3b75-05c9-446c-a168-230eb4672b00', UTC_TIMESTAMP());
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('6c833097-d117-4215-888e-8f4d882af274', 'Blacksmith', '7a2b3b75-05c9-446c-a168-230eb4672b00', UTC_TIMESTAMP());
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('34674251-fac9-46b6-b7de-6223dc87037f', 'Tailor', '7a2b3b75-05c9-446c-a168-230eb4672b00', UTC_TIMESTAMP());
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('f846e0e1-789a-4d29-b1bc-17aa45f1955e', 'Alchemist', '7a2b3b75-05c9-446c-a168-230eb4672b00', UTC_TIMESTAMP());
INSERT INTO `Role` (`ID`, `Name`, `RoleTypeID`, `TimeCreated`) VALUES ('9ede5485-8cfe-4bd5-9e2e-73c8d0768582', 'Woodcutter', '7a2b3b75-05c9-446c-a168-230eb4672b00', UTC_TIMESTAMP());

-- Insert Feat
INSERT INTO `Feat` (`ID`, `Name`, `TimeCreated`) VALUES ('bc6208be-93c8-4a59-abf6-39aaabc10d11', 'First quest completed', UTC_TIMESTAMP());
INSERT INTO `Feat` (`ID`, `Name`, `TimeCreated`) VALUES ('cf21a7af-c543-49fa-bb90-f72f7702a93f', '10 quests completed', UTC_TIMESTAMP());
INSERT INTO `Feat` (`ID`, `Name`, `TimeCreated`) VALUES ('b5bfb9f9-f48f-4081-bb66-e4648fd231c8', '100 quests completed', UTC_TIMESTAMP());
INSERT INTO `Feat` (`ID`, `Name`, `TimeCreated`) VALUES ('11ead1e6-fbff-4d65-a45f-86214195ea07', '1000 quests completed', UTC_TIMESTAMP());
INSERT INTO `Feat` (`ID`, `Name`, `TimeCreated`) VALUES ('a1eeb61d-21a7-4589-a843-970fee8c0718', 'First mission completed', UTC_TIMESTAMP());

-- Insert GuildUnitModifier
INSERT INTO `GuildUnitModifier` (`ID`, `Value`, `ArithmeticalTag`, `FeatID`, `Tag`, `TimeCreated`) 
VALUES ('937b150b-75f5-4dc2-98ca-d6e899e61cb7', -5000, 'Additive', 'bc6208be-93c8-4a59-abf6-39aaabc10d11', 'RefreshTime', UTC_TIMESTAMP());
INSERT INTO `GuildUnitModifier` (`ID`, `Value`, `ArithmeticalTag`, `FeatID`, `Tag`, `TimeCreated`) 
VALUES ('65f93928-2362-4a9b-ae59-dc90f1a5806f', -5000, 'Additive', 'cf21a7af-c543-49fa-bb90-f72f7702a93f', 'RefreshTime', UTC_TIMESTAMP());
INSERT INTO `GuildUnitModifier` (`ID`, `Value`, `ArithmeticalTag`, `FeatID`, `Tag`, `TimeCreated`) 
VALUES ('7945d7eb-51ae-46df-9c72-f0995b0aa632', -5000, 'Additive', 'b5bfb9f9-f48f-4081-bb66-e4648fd231c8', 'RefreshTime', UTC_TIMESTAMP());
INSERT INTO `GuildUnitModifier` (`ID`, `Value`, `ArithmeticalTag`, `FeatID`, `Tag`, `TimeCreated`) 
VALUES ('55ae9476-a181-471d-963e-06841ddaf83f', -5000, 'Additive', '11ead1e6-fbff-4d65-a45f-86214195ea07', 'RefreshTime', UTC_TIMESTAMP());
INSERT INTO `GuildUnitModifier` (`ID`, `Value`, `ArithmeticalTag`, `FeatID`, `Tag`, `TimeCreated`) 
VALUES ('8626af1c-77d6-4c40-aad0-b9a618446978', '1000', 'Additive', 'a1eeb61d-21a7-4589-a843-970fee8c0718', 'Experience', UTC_TIMESTAMP());

-- Insert GuildRoleModifier
INSERT INTO `GuildRoleModifier` (`ID`, `Value`, `ArithmeticalTag`, `FeatID`, `Tag`, `RoleTypeID`, `TimeCreated`) 
VALUES ('b88cd5cf-40f6-4bcc-a406-8a850793c868', 1, 'Additive', 'bc6208be-93c8-4a59-abf6-39aaabc10d11', 'Count', '6f4c1af6-5fa4-4b93-a0b7-38908a58eea6', UTC_TIMESTAMP());

INSERT INTO `GuildRoleModifier` (`ID`, `Value`, `ArithmeticalTag`, `FeatID`, `Tag`, `RoleTypeID`, `TimeCreated`) 
VALUES ('7e1a1b9f-a69e-4238-9e46-b6605302eb5a', 1, 'Additive', 'bc6208be-93c8-4a59-abf6-39aaabc10d11', 'Count', '7a2b3b75-05c9-446c-a168-230eb4672b00', UTC_TIMESTAMP());

-- Insert Guild-Feat
INSERT INTO `GuildFeat` (ID, GuildID, FeatID, TimeCreated) VALUES ('464f4032-bc99-4276-9c5e-19ce8eaa1f45', '33c8fcaa-302c-4d4e-9953-c0bb5839f3f5', 'bc6208be-93c8-4a59-abf6-39aaabc10d11', UTC_TIMESTAMP());
INSERT INTO `GuildFeat` (ID, GuildID, FeatID, TimeCreated) VALUES ('fc249197-088a-4d81-a3dd-67b653994285', '33c8fcaa-302c-4d4e-9953-c0bb5839f3f5', 'cf21a7af-c543-49fa-bb90-f72f7702a93f', UTC_TIMESTAMP());
INSERT INTO `GuildFeat` (ID, GuildID, FeatID, TimeCreated) VALUES ('fc98d629-3e6a-4ee9-a04d-b83eb96aed43', '33c8fcaa-302c-4d4e-9953-c0bb5839f3f5', 'a1eeb61d-21a7-4589-a843-970fee8c0718', UTC_TIMESTAMP());
