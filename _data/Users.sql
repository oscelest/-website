CREATE USER 'dev'@'localhost' IDENTIFIED WITH mysql_native_password BY 'dev';
GRANT ALL PRIVILEGES ON test.* TO 'dev'@'localhost' WITH GRANT OPTION;
CREATE USER 'test'@'localhost' IDENTIFIED WITH mysql_native_password BY 'password';
GRANT ALL PRIVILEGES ON test.* TO 'test'@'localhost' WITH GRANT OPTION;