# SQL Server Profiler Guide for Gamified Learning Platform

## Overview
This guide explains how to use SQL Server Profiler to monitor database operations performed by the application, and what SQL operations you can observe.

## Setting Up SQL Profiler

### 1. Launch SQL Server Profiler
- Open SQL Server Management Studio (SSMS)
- Go to **Tools** → **SQL Server Profiler**
- Or search for "SQL Server Profiler" in Windows Start menu

### 2. Create a New Trace
- Click **File** → **New Trace**
- Connect to your SQL Server instance: `(localdb)\MSSQLLocalDB`
- Select **Use Windows Authentication**

### 3. Configure Trace Properties
- **General Tab**:
  - Trace name: `GamifiedLearningPlatform_Trace`
  - Template: Select **TSQL_Replay** (or **Standard** for basic monitoring)
  - Save to file: Optional (recommended for analysis)
  - Enable trace stop time: Optional

- **Events Selection Tab**:
  - Enable the following events:
    - **Stored Procedures**:
      - `SP:StmtCompleted`
      - `SP:StmtStarting`
    - **TSQL**:
      - `SQL:BatchCompleted`
      - `SQL:BatchStarting`
      - `SQL:StmtCompleted`
      - `SQL:StmtStarting`
    - **Transactions**:
      - `TransactionLog`
    - **Errors and Warnings**:
      - `ErrorLog`
      - `Exception`

### 4. Add Column Filters
- Click **Column Filters...**
- Add filter for **DatabaseName**:
  - Like: `GamifiedLearning_CodeFirst`
  - This filters to show only operations on your application database

### 5. Start the Trace
- Click **Run** to start the trace
- Keep Profiler running while you use the application

## SQL Operations You Can See in Profiler

### When Using Code First EF Core (`CodeFirstEf`)

#### On Application Startup (Load Students)
```sql
-- Load all students
SELECT [s].[Id], [s].[FirstName], [s].[LastName], [s].[Email], [s].[TotalXp], [s].[Level]
FROM [Students] AS [s]

-- Load assignments for students
SELECT [a].[Id], [a].[Title], [a].[XpAward], [a].[IsCompleted], [a].[StudentId]
FROM [Assignments] AS [a]
WHERE [a].[StudentId] IN (@p0, @p1, ...)

-- Load badges for students
SELECT [s0].[Id], [s0].[Name], [s0].[StudentId]
FROM [StudentBadges] AS [s0]
WHERE [s0].[StudentId] IN (@p0, @p1, ...)
```

#### On Save Button Click (Save Students)
```sql
-- Check existing students (for update/insert logic)
SELECT [s].[Id], [s].[FirstName], [s].[LastName], [s].[Email], [s].[TotalXp], [s].[Level]
FROM [Students] AS [s]

-- Load related data
SELECT [a].[Id], [a].[Title], [a].[XpAward], [a].[IsCompleted], [a].[StudentId]
FROM [Assignments] AS [a]
WHERE [a].[StudentId] IN (@p0, @p1, ...)

SELECT [s0].[Id], [s0].[Name], [s0].[StudentId]
FROM [StudentBadges] AS [s0]
WHERE [s0].[StudentId] IN (@p0, @p1, ...)

-- INSERT new students
INSERT INTO [Students] ([Id], [FirstName], [LastName], [Email], [TotalXp], [Level])
VALUES (@p0, @p1, @p2, @p3, @p4, @p5)

-- UPDATE existing students
UPDATE [Students]
SET [FirstName] = @p0, [LastName] = @p1, [Email] = @p2, [TotalXp] = @p3, [Level] = @p4
WHERE [Id] = @p5

-- DELETE removed assignments
DELETE FROM [Assignments]
WHERE [Id] = @p0

-- INSERT new assignments
INSERT INTO [Assignments] ([Id], [Title], [XpAward], [IsCompleted], [StudentId])
VALUES (@p0, @p1, @p2, @p3, @p4)

-- UPDATE existing assignments
UPDATE [Assignments]
SET [Title] = @p0, [XpAward] = @p1, [IsCompleted] = @p2
WHERE [Id] = @p3

-- DELETE all badges for a student (then reinsert)
DELETE FROM [StudentBadges]
WHERE [StudentId] = @p0

-- INSERT new badges
INSERT INTO [StudentBadges] ([Id], [Name], [StudentId])
VALUES (@p0, @p1, @p2)

-- DELETE removed students (cascade deletes children)
DELETE FROM [Students]
WHERE [Id] = @p0
```

### When Using Db First EF Core (`DbFirstEf`)

Similar SQL operations as Code First, but you'll see:
- Same SELECT, INSERT, UPDATE, DELETE patterns
- Entity Framework generates similar SQL based on the model

## Key Features of EF Core

### EF Core (Code First & Db First)
- Uses parameterized queries with `@p0`, `@p1`, etc.
- Batch operations in single `SaveChanges()` call
- Automatic transaction management
- More complex SQL with JOINs for related data loading

## Useful Profiler Columns to Monitor

1. **TextData**: The actual SQL statement
2. **EventClass**: Type of event (SQL:BatchCompleted, etc.)
3. **Duration**: Execution time in milliseconds
4. **CPU**: CPU time used
5. **Reads**: Number of logical reads
6. **Writes**: Number of logical writes
7. **StartTime**: When the operation started
8. **SPID**: Server Process ID (session identifier)

## Tips for Analysis

1. **Filter by Duration**: Add a filter for `Duration > 100` to see slow queries
2. **Group by TextData**: Use "Group by" to see which queries run most frequently
3. **Save Trace to File**: Save traces for later analysis
4. **Replay Traces**: Use TSQL_Replay template to replay captured traces
5. **Monitor Transactions**: Watch for long-running transactions that might cause locks

## Common Issues to Watch For

1. **N+1 Query Problem**: Multiple SELECT statements for related data
   - Solution: EF Core uses `.Include()` to load related data in one query

2. **Missing Indexes**: Slow SELECT queries
   - Solution: Check execution plans, add indexes on frequently queried columns

3. **Transaction Timeouts**: Long-running transactions
   - Solution: Optimize save operations, use smaller batches

4. **Concurrency Conflicts**: "Expected to affect 1 row but actually affected 0"
   - Solution: Proper entity state management (fixed in current implementation)

## Example Trace Analysis

When you click "Save" button, you should see:
1. SELECT statements to load current state
2. UPDATE/INSERT statements for modified entities
3. DELETE statements for removed entities
4. All wrapped in a single SaveChanges call (EF Core manages transactions automatically)

The number of SQL statements will vary based on:
- Number of students being saved
- Number of assignments per student
- Number of badges per student
- Whether entities are new, updated, or deleted



