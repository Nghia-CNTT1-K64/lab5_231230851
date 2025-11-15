CREATE DATABASE QuanLyNhanVienWeb;

-- Sử dụng cơ sở dữ liệu vừa tạo
USE QuanLyNhanVienWeb;

-- Tạo bảng Employee
CREATE TABLE Employee (
    id INT PRIMARY KEY,                  
    fullName NVARCHAR(100) NOT NULL,     
    gender NVARCHAR(10) CHECK (gender IN ('Male','Female')), -- chỉ cho phép Male hoặc Female
    phone VARCHAR(15) UNIQUE,            
    email VARCHAR(100) UNIQUE,           
    salary DECIMAL(18,2) CHECK (salary >= 0), 
    status NVARCHAR(20)                  
);

INSERT INTO Employee (id, fullName, gender, phone, email, salary, status)
VALUES 
(1, N'Nguyen Van An', 'Male', '0901234567', 'an.nguyen@example.com', 12000000, N'Dang lam'),
(2, N'Tran Thi Binh', 'Female', '0912345678', 'binh.tran@example.com', 15000000, N'Dang lam'),
(3, N'Le Van Cuong', 'Male', '0923456789', 'cuong.le@example.com', 10000000, N'Nghi viec'),
(4, N'Pham Thi Dao', 'Female', '0934567890', 'dao.pham@example.com', 18000000, N'Dang lam'),
(5, N'Hoang Van Em', 'Male', '0945678901', 'em.hoang@example.com', 9000000, N'Dang lam');

select * from Employee










