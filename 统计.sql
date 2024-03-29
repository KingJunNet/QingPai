 --select *
 --from 
	-- (SELECT department,drum,Type1,Producer,Carvin,Price,
	-- (CASE WHEN TestEndDate is not null THEN TestEndDate ELSE TestStartDate END) as TaskDate
	--  FROM TaskTable where Finishstate='已完成' and TestStartDate is not null) taskView 
	--order by department,Drum,Type1,Producer,TaskDate
 
select department,Drum,Type1,Producer,COUNT(distinct(Carvin)) as CarCount,SUM(Price) as TotalPrice
 from  (SELECT department,drum,Type1,Producer,Carvin,Price,
(CASE WHEN TestEndDate is not null THEN TestEndDate ELSE TestStartDate END) as TaskDate
FROM TaskTable where Finishstate='已完成' and TestStartDate is not null) taskView 
  group by department,Drum,Type1,Producer order by department,Drum,Type1,Producer
 
 /*
 department,Drum,Type1,Producer,Carvin, COUNT(Carvin) as CarCount,
 SUM((CASE WHEN ISNUMERIC(Price)=1 THEN CONVERT(real,Price) ELSE 0 END)) as TotalPrice
 */