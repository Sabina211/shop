create table client_info (
id int not null identity(1,1) primary key,
surname NVARCHAR(50) not null,
first_name NVARCHAR(50) not null,
patronymic NVARCHAR(50),
phone_number NVARCHAR(50),
email NVARCHAR(50) not null
)

insert into client_info (surname, first_name, patronymic, phone_number, email )
values (N'Карлов123', N'Карл', N'Карлович', '890900000001', 'Iamkarl@gmail.com');


insert into client_info (surname,first_name, patronymic, phone_number, email )
values (N'Карлов', N'Марк', N'Карлович', '890900000001', 'Iammark@gmail.com');

insert into client_info (surname,first_name, patronymic, phone_number, email )
values (N'Одинов', N'Марк', N'Маркович', '890900000003', 'Iammark1@gmail.com');

update  client_info set first_name=N'Ольга', surname=N'Павловна' where id=2

select * from client_info 

drop table client_info;


