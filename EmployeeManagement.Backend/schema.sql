-- ========================================================
-- Employee Management System - MySQL Database Schema & Seed
-- ========================================================

CREATE DATABASE IF NOT EXISTS `employeedb` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `employeedb`;

-- 1. Table: ParentDepartments
CREATE TABLE IF NOT EXISTS `ParentDepartments` (
    `DepartmentId` INT NOT NULL AUTO_INCREMENT,
    `DepartmentName` VARCHAR(150) NOT NULL,
    `DepartmentLogo` VARCHAR(500) NULL,
    `CreatedDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `UpdatedDate` DATETIME NULL,
    PRIMARY KEY (`DepartmentId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 2. Table: ChildDepartments
CREATE TABLE IF NOT EXISTS `ChildDepartments` (
    `ChildDepartmentId` INT NOT NULL AUTO_INCREMENT,
    `ChildDepartmentName` VARCHAR(150) NOT NULL,
    `ChildDepartmentLogo` VARCHAR(500) NULL,
    `ParentDepartmentId` INT NOT NULL,
    `CreatedDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `UpdatedDate` DATETIME NULL,
    PRIMARY KEY (`ChildDepartmentId`),
    CONSTRAINT `FK_ChildDepartments_ParentDepartments` 
        FOREIGN KEY (`ParentDepartmentId`) 
        REFERENCES `ParentDepartments` (`DepartmentId`) 
        ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 3. Table: Employees
CREATE TABLE IF NOT EXISTS `Employees` (
    `EmployeeId` INT NOT NULL AUTO_INCREMENT,
    `EmployeeName` VARCHAR(150) NOT NULL,
    `ContactNo` VARCHAR(20) NOT NULL,
    `EmailId` VARCHAR(150) NOT NULL,
    `Password` VARCHAR(255) NOT NULL,
    `Gender` VARCHAR(20) NULL,
    `Role` VARCHAR(50) NOT NULL DEFAULT 'Employee',
    `DeptId` INT NULL,
    `CreatedDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `UpdatedDate` DATETIME NULL,
    PRIMARY KEY (`EmployeeId`),
    UNIQUE KEY `UQ_Employees_EmailId` (`EmailId`),
    CONSTRAINT `FK_Employees_ChildDepartments` 
        FOREIGN KEY (`DeptId`) 
        REFERENCES `ChildDepartments` (`ChildDepartmentId`) 
        ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 4. Table: LeaveRequests
CREATE TABLE IF NOT EXISTS `LeaveRequests` (
    `LeaveId` INT NOT NULL AUTO_INCREMENT,
    `EmployeeId` INT NOT NULL,
    `EmployeeName` VARCHAR(150) NOT NULL,
    `LeaveType` VARCHAR(50) NOT NULL DEFAULT 'Casual Leave',
    `StartDate` DATETIME NOT NULL,
    `EndDate` DATETIME NOT NULL,
    `DaysCount` INT NOT NULL DEFAULT 1,
    `Reason` VARCHAR(500) NULL,
    `Status` VARCHAR(50) NOT NULL DEFAULT 'Pending',
    `CreatedDate` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `UpdatedDate` DATETIME NULL,
    PRIMARY KEY (`LeaveId`),
    CONSTRAINT `FK_LeaveRequests_Employees`
        FOREIGN KEY (`EmployeeId`)
        REFERENCES `Employees` (`EmployeeId`)
        ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ========================================================
-- Seed Data
-- ========================================================

-- Parent Departments
INSERT INTO `ParentDepartments` (`DepartmentId`, `DepartmentName`, `DepartmentLogo`, `CreatedDate`)
VALUES 
    (1, 'Human Resources', 'https://images.unsplash.com/photo-1551836022-d5d88e9218df?w=100&auto=format&fit=crop&q=60', NOW()),
    (2, 'Engineering & Technology', 'https://images.unsplash.com/photo-1581091226825-a6a2a5aee158?w=100&auto=format&fit=crop&q=60', NOW()),
    (3, 'Finance & Accounting', 'https://images.unsplash.com/photo-1554224155-8d04cb21cd6c?w=100&auto=format&fit=crop&q=60', NOW())
ON DUPLICATE KEY UPDATE `DepartmentName`=VALUES(`DepartmentName`);

-- Child Departments
INSERT INTO `ChildDepartments` (`ChildDepartmentId`, `ChildDepartmentName`, `ChildDepartmentLogo`, `ParentDepartmentId`, `CreatedDate`)
VALUES
    (1, 'Talent Acquisition', 'https://images.unsplash.com/photo-1521737604893-d14cc237f11d?w=100&auto=format&fit=crop&q=60', 1, NOW()),
    (2, 'Employee Relations', 'https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=100&auto=format&fit=crop&q=60', 1, NOW()),
    (3, 'Software Development', 'https://images.unsplash.com/photo-1498050108023-c5249f4df085?w=100&auto=format&fit=crop&q=60', 2, NOW()),
    (4, 'Quality Assurance & DevOps', 'https://images.unsplash.com/photo-1518770660439-4636190af475?w=100&auto=format&fit=crop&q=60', 2, NOW()),
    (5, 'Payroll & Taxation', 'https://images.unsplash.com/photo-1450133064473-71024230f91b?w=100&auto=format&fit=crop&q=60', 3, NOW())
ON DUPLICATE KEY UPDATE `ChildDepartmentName`=VALUES(`ChildDepartmentName`);

-- Seed Employees (Admin and Sample Employee)
INSERT INTO `Employees` (`EmployeeId`, `EmployeeName`, `ContactNo`, `EmailId`, `Password`, `Gender`, `Role`, `DeptId`, `CreatedDate`)
VALUES
    (1, 'Admin User', '9876543210', 'admin@company.com', 'admin', 'Male', 'Admin', 3, NOW()),
    (2, 'Raj Bosh', '9123456789', 'raj.bosh@company.com', 'password123', 'Male', 'Employee', 3, NOW())
ON DUPLICATE KEY UPDATE `EmployeeName`=VALUES(`EmployeeName`);
