SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[SP_S_FASSESS_DEPT_LEVEL]
	@ASSESS_YEAR varchar(4), 
	@ASSESS_TYPE varchar(1), 
	@DEPT_NO varchar(7), 
	@DEPT_LEVEL varchar(3),
	@LEVEL_RATE varchar(20),
	@IS_V_DEPT varchar(1),
	@USERID varchar(20),
	@FUNCID varchar(30)
as
BEGIN
   DECLARE @dept_name varchar(60);
   DECLARE @head_emp_id varchar(5);
   DECLARE @top_dept_no varchar(7);
   DECLARE @top_dept_name varchar(60);
   DECLARE @top_dept_level varchar(3);
   DECLARE @top_head_emp_id varchar(5);
   DECLARE @top_head_emp_id_1 varchar(5);
   DECLARE @top_head_emp_id_2 varchar(5);
   DECLARE @top_level_rate varchar(20);
   DECLARE @sub_dept_no varchar(7);
   DECLARE @sub_dept_name varchar(60);
   DECLARE @sub_dept_level varchar(3);
   DECLARE @sub_head_emp_id varchar(5);
   DECLARE @sub_level_rate varchar(20);
   DECLARE @sub_index int;
   DECLARE @sub_count int;
   DECLARE @sub_vr_count int;
   DECLARE @sub_is_dept_level varchar(1);
   DECLARE @upd_dept_no varchar(7);
   DECLARE @sub_is_v_dept varchar(1);
   if @DEPT_NO='' 
      begin
	  --刪除已產生TB_S_M_FOREIGN_DEPT_LEVEL
	  DELETE FROM TB_S_M_FOREIGN_DEPT_LEVEL WHERE ASSESS_YEAR=@assess_year and ASSESS_TYPE=@assess_type;

	  set @sub_index=1;
	  --處理協理,二階及部長
	  DECLARE cur_UP_20_DEPT CURSOR FOR
	  select DEPT_NO, DEPT_NAME, HEAD_EMP_ID,HEAD_EMP_ID_1,HEAD_EMP_ID_2 from VW_S_M_TOP_SEC_HEAD where DEPT_NO<>'KS00000';
	  --開啓CURSOR
		OPEN cur_UP_20_DEPT;
		FETCH NEXT FROM cur_UP_20_DEPT
		INTO
			 @top_dept_no, @top_dept_name, @top_head_emp_id, @top_head_emp_id_1, @top_head_emp_id_2; 
		WHILE(@@FETCH_STATUS=0)
		BEGIN
			--協理
			-- set @top_level_rate = right('0'+cast(@sub_index as varchar),2);
			-- insert into TB_S_M_FOREIGN_DEPT_LEVEL(ASSESS_YEAR, ASSESS_TYPE, DEPT_NO, DEPT_NAME, DEPT_LEVEL ,HEAD_EMP_ID, LEVEL_RATE,IS_V_DEPT,
            --                            CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)values
			--						   (@ASSESS_YEAR, @ASSESS_TYPE, @top_dept_no, @top_dept_name, '10', @top_head_emp_id_2, @top_level_rate,@IS_V_DEPT,	
			--						   @USERID,getDate(),@USERID,getDate(), @FUNCID);
			--二階理事
			--if @top_head_emp_id_1<>''
			--begin
			--	set @top_level_rate = @top_level_rate+'01';
			--	 insert into TB_S_M_FOREIGN_DEPT_LEVEL(ASSESS_YEAR, ASSESS_TYPE,DEPT_NO, DEPT_NAME, DEPT_LEVEL ,HEAD_EMP_ID, LEVEL_RATE,IS_V_DEPT,
            --                            CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)values
			--						   (@ASSESS_YEAR, @ASSESS_TYPE, @top_dept_no, @top_dept_name, '15', @top_head_emp_id_2, @top_level_rate,@IS_V_DEPT,									   
			--						   @USERID,getDate(),@USERID,getDate(), @FUNCID);
			--end;
			--部長
			set @top_level_rate = @top_level_rate+'01';
			
			--print '20-'+@top_dept_no;
			EXECUTE dbo.SP_S_FASSESS_DEPT_LEVEL @ASSESS_YEAR, @ASSESS_TYPE, @top_dept_no, '20' ,@top_level_rate, @IS_V_DEPT, @USERID, @FUNCID;

		 set @sub_index = @sub_index+1;
		 FETCH NEXT FROM cur_UP_20_DEPT
		INTO
			 @top_dept_no, @top_dept_name, @top_head_emp_id, @top_head_emp_id_1, @top_head_emp_id_2; 
		END;
		--關閉&釋放cursor
		CLOSE cur_UP_20_DEPT;
		DEALLOCATE cur_UP_20_DEPT;
	  end;

 if @DEPT_NO<>''
	begin  
	SET @sub_vr_count=0;
   if @DEPT_LEVEL='20'
	begin
	create table #tempDeptLevel
		(
			UP_DEPT_NO varchar(7),
			DEPT_NO varchar(7),
			DEPT_NAME varchar(60), 
			DEPT_LEVEL varchar(3), 
			HEAD_EMP_ID varchar(5),
			IS_V_DEPT varchar(1)
		)
	
	end;
   --取得部門資料
   if(LEFT(@DEPT_NO,1)='Z')
   BEGIN
   select distinct @upd_dept_no=C.dept_no,@dept_name=D.dept_name,@head_emp_id=B.HEAD_EMP_ID 
	from TB_H_M_DEPT_ORG A LEFT JOIN 
		 TB_H_M_DEPT B ON A.DEPT_NO=B.DEPT_NO LEFT JOIN 
		 TB_H_M_DEPT_ORG C ON A.UP_DEPT_NO=C.DEPT_NO  LEFT JOIN 
		 TB_H_M_DEPT D ON C.DEPT_NO=D.DEPT_NO
	WHERE  A.DEPT_NO=@DEPT_NO
		  and A.START_DT <= GETDATE() and A.END_DT >=GETDATE()
	      and C.START_DT <= GETDATE() and C.END_DT >=GETDATE() and B.START_DT <= GETDATE() and B.END_DT >=GETDATE() and D.START_DT <= GETDATE() and D.END_DT >=GETDATE()
   END
   ELSE
   BEGIN
   select @upd_dept_no=dept_no,@dept_name=DEPT_NAME,@head_emp_id=HEAD_EMP_ID from TB_H_R_DEPT_DATA_AD where DEPT_NO=@DEPT_NO
   END
   --新增至TB_S_M_FOREIGN_DEPT_LEVEL
   insert into TB_S_M_FOREIGN_DEPT_LEVEL(ASSESS_YEAR, ASSESS_TYPE,DEPT_NO, DEPT_NAME, DEPT_LEVEL ,HEAD_EMP_ID, LEVEL_RATE,
                                        IS_V_DEPT,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)values
									   (@ASSESS_YEAR, @ASSESS_TYPE, @upd_dept_no, @dept_name, @DEPT_LEVEL, @head_emp_id, @level_rate,
									   @IS_V_DEPT,@USERID,getDate(),@USERID,getDate(), @FUNCID);
   if( @DEPT_LEVEL='20')
   BEGIN
	select @sub_count=count(*)
	from TB_H_M_DEPT_ORG A LEFT JOIN TB_H_M_DEPT B ON A.DEPT_NO=B.DEPT_NO 
	WHERE  A.UP_DEPT_NO=@DEPT_NO and A.START_DT <= GETDATE() and A.END_DT >=GETDATE() and B.START_DT <= GETDATE() and B.END_DT >=GETDATE() 
   END
   ELSE
   BEGIN
    if(LEFT(@DEPT_NO,1)='Z')
	BEGIN
		select @sub_count=count(*)
		from TB_H_M_DEPT_ORG A LEFT JOIN TB_H_M_DEPT B ON A.DEPT_NO=B.DEPT_NO 
		WHERE  A.UP_DEPT_NO=@DEPT_NO and A.START_DT <= GETDATE() and A.END_DT >=GETDATE() and B.START_DT <= GETDATE() and B.END_DT >=GETDATE() AND LEFT(A.DEPT_NO,1)<>'Z'
	END
	ELSE
	BEGIN
		select @sub_count=count(*)  FROM TB_H_R_DEPT_DATA_AD		WHERE up_dept_no=@DEPT_NO;
    END
   END;
   --print cast(@sub_count as varchar)+'-'+@DEPT_NO;
   if @sub_count>0
	begin
	   --將子部門存入temp table
	   if(@DEPT_LEVEL='20')
	   BEGIN
	    insert into #tempDeptLevel(UP_DEPT_NO, DEPT_NO, DEPT_NAME, DEPT_LEVEL, HEAD_EMP_ID, IS_V_DEPT)
		select @DEPT_NO, A.DEPT_NO, @dept_name, A.DEPT_LEVEL, B.HEAD_EMP_ID ,CASE WHEN LEFT(A.DEPT_NO,1)='Z' THEN 'Y' ELSE 'N'END
		from TB_H_M_DEPT_ORG A LEFT JOIN TB_H_M_DEPT B ON A.DEPT_NO=B.DEPT_NO 
		WHERE  A.UP_DEPT_NO=@DEPT_NO and A.START_DT <= GETDATE() and A.END_DT >=GETDATE() and B.START_DT <= GETDATE() and B.END_DT >=GETDATE() 
	   END
	   else
	   BEGIN
		 if(LEFT(@DEPT_NO,1)='Z')
			BEGIN
				insert into #tempDeptLevel(UP_DEPT_NO, DEPT_NO, DEPT_NAME, DEPT_LEVEL, HEAD_EMP_ID,IS_V_DEPT)
				select @DEPT_NO, A.DEPT_NO, @dept_name, A.DEPT_LEVEL, B.HEAD_EMP_ID ,CASE WHEN LEFT(A.DEPT_NO,1)='Z' THEN 'Y' ELSE 'N'END
				from TB_H_M_DEPT_ORG A LEFT JOIN TB_H_M_DEPT B ON A.DEPT_NO=B.DEPT_NO 
				WHERE  A.UP_DEPT_NO=@DEPT_NO and A.START_DT <= GETDATE() and A.END_DT >=GETDATE() and B.START_DT <= GETDATE() and B.END_DT >=GETDATE() and LEFT(A.DEPT_NO,1)<>'Z'
			END
			ELSE
			BEGIN
			   insert into #tempDeptLevel(UP_DEPT_NO, DEPT_NO, DEPT_NAME, DEPT_LEVEL, HEAD_EMP_ID,IS_V_DEPT)
			   SELECT @DEPT_NO, DEPT_NO, DEPT_NAME, DEPT_LEVEL, HEAD_EMP_ID,CASE WHEN LEFT(DEPT_NO,1)='Z' THEN 'Y' ELSE 'N'END
					FROM TB_H_R_DEPT_DATA_AD
					WHERE up_dept_no=@DEPT_NO;
		   END
	   END
	   declare @countTemp int
	   select @countTemp = count(*) from #tempDeptLevel where UP_DEPT_NO=@DEPT_NO;
		--print @DEPT_NO+'-'+cast(@countTemp as varchar);
		--set rowcount 1
		set @sub_index=1;
		WHILE(@countTemp > 0)
			BEGIN
			
			select @sub_dept_no = DEPT_NO, @sub_dept_name = DEPT_NAME, @sub_dept_level=DEPT_LEVEL, @sub_head_emp_id=HEAD_EMP_ID ,@sub_is_v_dept=IS_V_DEPT
			from #tempDeptLevel
			where UP_DEPT_NO=@DEPT_NO order by DEPT_NO ;
				--To Something
			set @sub_level_rate=''
			set @sub_level_rate = @LEVEL_RATE+ RIGHT('0'+cast(@sub_index as varchar),2);
			 --print @sub_level_rate+'-'+@sub_dept_no+'-'+@sub_is_v_dept;

			 EXECUTE dbo.SP_S_ASSESS_DEPT_LEVEL @ASSESS_YEAR, @ASSESS_TYPE, @sub_dept_no, @sub_dept_level ,@sub_level_rate,@sub_is_v_dept, @USERID, @FUNCID;

			delete from #tempDeptLevel where UP_DEPT_NO=@DEPT_NO and DEPT_NO=@sub_dept_no;

			set @sub_index=@sub_index+1;
			select @countTemp = count(*) from #tempDeptLevel where UP_DEPT_NO=@DEPT_NO;

			END;

			if @DEPT_LEVEL='20'
			begin
			drop table #tempDeptLevel
			--把select的預設比數恢復正常
			set rowcount 0
			end;
	END;
 end;
END;






GO
