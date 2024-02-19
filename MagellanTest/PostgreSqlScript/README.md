Put the PostgreSQL script of Part 1 here.

-- Create the Part database
CREATE DATABASE Part;

-- Connect to the Part database
\c Part

-- Create the item table
CREATE TABLE item (
    id SERIAL PRIMARY KEY,
    item_name VARCHAR(50) NOT NULL,
    parent_item INTEGER,
    cost INTEGER NOT NULL,
    req_date DATE NOT NULL,
    FOREIGN KEY (parent_item) REFERENCES item(id)
);

-- Insert data into the item table
INSERT INTO item (item_name, parent_item, cost, req_date)
VALUES 
    ('Item1', null, 500, '2024-02-20'),
    ('Sub1', 1, 200, '2024-02-10'),
    ('Sub2', 1, 300, '2024-01-05'),
    ('Sub3', 2, 300, '2024-01-02'),
    ('Sub4', 2, 400, '2024-01-02'),
    ('Item2', null, 600, '2024-03-15'),
    ('Sub1', 6, 200, '2024-02-25');


-- Making get total cost function
CREATE OR REPLACE FUNCTION Get_Total_Cost(item_name VARCHAR)
RETURNS INTEGER AS $$
DECLARE
    start_id INTEGER;
BEGIN
    -- Find the ID of the top-level item with the given item_name
    SELECT id INTO start_id FROM item AS i WHERE i.item_name = Get_Total_Cost.item_name AND i.parent_item IS NULL;

    -- Return NULL if the item is not a top-level item or doesn't exist
    IF start_id IS NULL THEN
        RETURN NULL;
    END IF;

    -- Recursive CTE to calculate the total cost
    WITH RECURSIVE Cost_CTE AS (
        SELECT
            id,
            cost
        FROM
            item
        WHERE
            id = start_id  -- Start from the identified top-level item

        UNION ALL

        SELECT
            i.id,
            i.cost
        FROM
            item i
            INNER JOIN Cost_CTE cte ON i.parent_item = cte.id
    )
$$ LANGUAGE plpgsql; INTO start_id FROM Cost_CTE;  -- Aggregate the total cost
CREATE FUNCTION