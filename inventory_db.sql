
DROP TABLE IF EXISTS Purchase_Items;
DROP TABLE IF EXISTS Purchases;
DROP TABLE IF EXISTS Order_Items;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Categories;
DROP TABLE IF EXISTS Suppliers;
DROP TABLE IF EXISTS Customers;

CREATE TABLE Categories (
    category_id   INT           NOT NULL AUTO_INCREMENT,
    category_name VARCHAR(100)  NOT NULL,
    PRIMARY KEY (category_id)
);

CREATE TABLE Suppliers (
    supplier_id  INT           NOT NULL AUTO_INCREMENT,
    name         VARCHAR(150)  NOT NULL,
    contact_info VARCHAR(255),
    address      VARCHAR(255),
    PRIMARY KEY (supplier_id)
);

CREATE TABLE Customers (
    customer_id INT           NOT NULL AUTO_INCREMENT,
    name        VARCHAR(150)  NOT NULL,
    phone       VARCHAR(20),
    email       VARCHAR(150),
    PRIMARY KEY (customer_id)
);


CREATE TABLE Products (
    product_id     INT             NOT NULL AUTO_INCREMENT,
    name           VARCHAR(150)    NOT NULL,
    description    TEXT,
    category_id    INT             NOT NULL,
    price          DECIMAL(10, 2)  NOT NULL DEFAULT 0.00,
    stock_quantity INT             NOT NULL DEFAULT 0,
    supplier_id    INT             NOT NULL,
    PRIMARY KEY (product_id),
    CONSTRAINT fk_product_category
        FOREIGN KEY (category_id) REFERENCES Categories (category_id)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_product_supplier
        FOREIGN KEY (supplier_id) REFERENCES Suppliers (supplier_id)
        ON UPDATE CASCADE ON DELETE RESTRICT
);


CREATE TABLE Orders (
    order_id     INT             NOT NULL AUTO_INCREMENT,
    customer_id  INT             NOT NULL,
    order_date   DATE            NOT NULL DEFAULT (CURRENT_DATE),
    total_amount DECIMAL(12, 2)  NOT NULL DEFAULT 0.00,
    PRIMARY KEY (order_id),
    CONSTRAINT fk_order_customer
        FOREIGN KEY (customer_id) REFERENCES Customers (customer_id)
        ON UPDATE CASCADE ON DELETE RESTRICT
);

CREATE TABLE Order_Items (
    order_item_id INT             NOT NULL AUTO_INCREMENT,
    order_id      INT             NOT NULL,
    product_id    INT             NOT NULL,
    quantity      INT             NOT NULL DEFAULT 1,
    price         DECIMAL(10, 2)  NOT NULL,
    PRIMARY KEY (order_item_id),
    CONSTRAINT fk_orderitem_order
        FOREIGN KEY (order_id) REFERENCES Orders (order_id)
        ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT fk_orderitem_product
        FOREIGN KEY (product_id) REFERENCES Products (product_id)
        ON UPDATE CASCADE ON DELETE RESTRICT
);

CREATE TABLE Purchases (
    purchase_id   INT  NOT NULL AUTO_INCREMENT,
    supplier_id   INT  NOT NULL,
    purchase_date DATE NOT NULL DEFAULT (CURRENT_DATE),
    PRIMARY KEY (purchase_id),
    CONSTRAINT fk_purchase_supplier
        FOREIGN KEY (supplier_id) REFERENCES Suppliers (supplier_id)
        ON UPDATE CASCADE ON DELETE RESTRICT
);


CREATE TABLE Purchase_Items (
    purchase_item_id INT             NOT NULL AUTO_INCREMENT,
    purchase_id      INT             NOT NULL,
    product_id       INT             NOT NULL,
    quantity         INT             NOT NULL DEFAULT 1,
    cost_price       DECIMAL(10, 2)  NOT NULL,
    PRIMARY KEY (purchase_item_id),
    CONSTRAINT fk_purchaseitem_purchase
        FOREIGN KEY (purchase_id) REFERENCES Purchases (purchase_id)
        ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT fk_purchaseitem_product
        FOREIGN KEY (product_id) REFERENCES Products (product_id)
        ON UPDATE CASCADE ON DELETE RESTRICT
);


CREATE INDEX idx_products_category  ON Products      (category_id);
CREATE INDEX idx_products_supplier  ON Products      (supplier_id);
CREATE INDEX idx_orders_customer    ON Orders        (customer_id);
CREATE INDEX idx_orderitems_order   ON Order_Items   (order_id);
CREATE INDEX idx_orderitems_product ON Order_Items   (product_id);
CREATE INDEX idx_purchases_supplier ON Purchases     (supplier_id);
CREATE INDEX idx_purchaseitems_pur  ON Purchase_Items (purchase_id);
CREATE INDEX idx_purchaseitems_prod ON Purchase_Items (product_id);
