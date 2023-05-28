CREATE TABLE `Region`(
    `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(255) NOT NULL
);
ALTER TABLE
    `Region` ADD PRIMARY KEY(`Id`);
CREATE TABLE `Owner`(
    `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(255) NOT NULL,
    `Gym` VARCHAR(255) NOT NULL,
    `RegionId` INT NOT NULL
);
ALTER TABLE
    `Owner` ADD PRIMARY KEY(`Id`);
CREATE TABLE `PokemonCategory`(
    `PokemonId` INT NOT NULL,
    `CategoryId` INT NOT NULL
);
CREATE TABLE `PokemonOwner`(
    `PokemonId` INT NOT NULL,
    `OwnerId` INT NOT NULL
);
CREATE TABLE `Category`(
    `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(255) NOT NULL
);
ALTER TABLE
    `Category` ADD PRIMARY KEY(`Id`);
CREATE TABLE `Reviewer`(
    `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `FirstName` VARCHAR(255) NOT NULL,
    `LastName` VARCHAR(255) NOT NULL
);
ALTER TABLE
    `Reviewer` ADD PRIMARY KEY(`Id`);
CREATE TABLE `Pokemon`(
    `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(255) NOT NULL,
    `CatchDate` DATETIME NOT NULL
);
ALTER TABLE
    `Pokemon` ADD PRIMARY KEY(`Id`);
CREATE TABLE `Review`(
    `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `Title` VARCHAR(255) NOT NULL,
    `Text` VARCHAR(510) NOT NULL,
    `PokemonId` INT NOT NULL,
    `ReviewerId` INT NOT NULL
);
ALTER TABLE
    `Review` ADD PRIMARY KEY(`Id`);
ALTER TABLE
    `Owner` ADD CONSTRAINT `owner_regionid_foreign` FOREIGN KEY(`RegionId`) REFERENCES `Region`(`Id`);
ALTER TABLE
    `PokemonOwner` ADD CONSTRAINT `pokemonowner_ownerid_foreign` FOREIGN KEY(`OwnerId`) REFERENCES `Owner`(`Id`);
ALTER TABLE
    `Review` ADD CONSTRAINT `review_reviewerid_foreign` FOREIGN KEY(`ReviewerId`) REFERENCES `Reviewer`(`Id`);
ALTER TABLE
    `Pokemon` ADD CONSTRAINT `pokemon_id_foreign` FOREIGN KEY(`Id`) REFERENCES `Review`(`Id`);
ALTER TABLE
    `PokemonOwner` ADD CONSTRAINT `pokemonowner_pokemonid_foreign` FOREIGN KEY(`PokemonId`) REFERENCES `Pokemon`(`Id`);
ALTER TABLE
    `PokemonCategory` ADD CONSTRAINT `pokemoncategory_pokemonid_foreign` FOREIGN KEY(`PokemonId`) REFERENCES `Pokemon`(`Id`);
ALTER TABLE
    `PokemonCategory` ADD CONSTRAINT `pokemoncategory_categoryid_foreign` FOREIGN KEY(`CategoryId`) REFERENCES `Category`(`Id`);