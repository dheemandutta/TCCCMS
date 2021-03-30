CREATE TABLE TblName (ClmnName VARCHAR(100));
----------------------------------------------------------
sp_configure 'show advanced options', '1'
RECONFIGURE
----------------------------------------------------------
sp_configure 'xp_cmdshell', '1' 
RECONFIGURE
----------------------------------------------------------
INSERT INTO TblName
EXEC xp_cmdshell 'dir /B "E:\All Projects\TCCCMS-master\FormsForVol8\Section 2 - Management of ship personnel"';
-----------------------------------------------------------------------------------------------------------------
select * from TblName

