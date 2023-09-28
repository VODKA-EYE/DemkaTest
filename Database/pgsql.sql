set schema 'public';
-- SQLINES LICENSE FOR EVALUATION USE ONLY
create sequence role_seq;

create table roles
(
  RoleID int primary key default nextval ('role_seq'),
  RoleName varchar(100) not null
);
-- SQLINES LICENSE FOR EVALUATION USE ONLY
create sequence user_seq;

create table users
(
  UserID int primary key default nextval ('user_seq'),
  UserSurname varchar(100) not null,
  UserName varchar(100) not null,
  UserPatronymic varchar(100) not null,
  UserLogin text not null,
  UserPassword text not null,
  UserRole int not null,
  foreign key (UserRole) references roles(RoleID) 
);


-- SQLINES LICENSE FOR EVALUATION USE ONLY
create sequence company_seq;

create table companies
(
  CompanyID int primary key default nextval ('company_seq'),
  CompanyName varchar(100) not null
);

create sequence product_category_seq;

create table product_categories
(
  ProductCategoryID int primary key default nextval ('product_category_seq'),
  ProductCategoryName varchar(100) not null
);

create table products
(
  ProductArticleNumber varchar(100) primary key,
  ProductName text not null,
  ProductDescription text not null,
  ProductCategory int not null,
  ProductPhoto bytea not null,
  ProductManufacturer int not null,
  ProductDeliveler int not null,
  ProductCost decimal(19,4) not null,
  ProductDiscountAmount smallint null,
  ProductQuantityInStock int not null,
  foreign key (ProductCategory) references product_categories(ProductCategoryID),
  foreign key (ProductManufacturer) references companies(CompanyID),
  foreign key (ProductDeliveler) references companies(CompanyID)
);
-- SQLINES LICENSE FOR EVALUATION USE ONLY

create sequence order_pickup_point_seq;

create table order_pickup_point
(
  OrderPickupPointId int primary key default nextval ('order_pickup_point_seq'),
  OrderPickupPointName varchar(200) not null
);

create sequence order_status_seq;

create table order_statuses
(
  StatusID int primary key default nextval ('order_status_seq'),
  StatusName varchar(100) not null
);

create sequence order_seq;

create table orders
(
  OrderID int primary key default nextval ('order_seq'),
  OrderStatus int not null,
  OrderDate timestamp(0) not null,
  OrderDeliveryDate timestamp(0) not null,
  OrderPickupPoint int not null,
  OrderClient int,
  OrderPickupCode int not null,
  foreign key (OrderPickupPoint) references order_pickup_point(OrderPickupPointId),
  foreign key (OrderStatus) references order_statuses(StatusID),
  foreign key (OrderClient) references users(UserID)
);
-- SQLINES LICENSE FOR EVALUATION USE ONLY
create table ordered_products
(
  OrderID int not null,
  ProductArticleNumber varchar(100)  not null,
  ProductAmount int not null,
  Primary key (OrderID,ProductArticleNumber),
  foreign key (OrderID) references orders(OrderID),
  foreign key (ProductArticleNumber) references products(ProductArticleNumber)
);