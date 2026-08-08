-- Fix gender field length so it can store values like N'Khác'
-- Run this on your WebsiteQLBanDongHo database

ALTER TABLE KHACHHANG
ALTER COLUMN GIOITINH NVARCHAR(10) NULL;
