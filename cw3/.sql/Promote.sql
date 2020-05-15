CREATE PROCEDURE Promote
	@Semester int,
	@Study varchar(50)
AS
	BEGIN 
		DECLARE @EnrollmentId int;
		DECLARE @EnrollmentIdForUpdate int;
		DECLARE @IdStudy int;
		DECLARE @NextEnrollmentId int;

		SELECT @EnrollmentId = e.IdEnrollment FROM Enrollment e 
		INNER JOIN Studies s ON s.IdStudy = e.IdStudy
		WHERE s.Name = @Study AND e.Semester = @Semester; 


		SELECT @EnrollmentIdForUpdate = e.IdEnrollment FROM Enrollment e 
		INNER JOIN Studies s ON s.IdStudy = e.IdStudy
		WHERE s.Name = @Study AND e.Semester = @Semester + 1;
	
		SELECT @IdStudy = s.IdStudy FROM Studies s WHERE s.Name = @Study;

		IF @EnrollmentIdForUpdate IS NOT NULL
		BEGIN	
			BEGIN TRAN
				BEGIN TRY
					UPDATE Student SET IdEnrollment = @EnrollmentIdForUpdate WHERE IdEnrollment = @EnrollmentId;
			
			        COMMIT TRAN
					RETURN @EnrollmentIdForUpdate;
				END TRY
				BEGIN CATCH
					ROLLBACK TRAN
				END CATCH
			END
		ELSE 
			BEGIN
				BEGIN TRAN
					BEGIN TRY
						SELECT @NextEnrollmentId = (MAX(e.IdEnrollment) + 1) FROM Enrollment e
						INSERT INTO Enrollment(IdEnrollment,IdStudy,Semester, StartDate)
						VALUES (@NextEnrollmentId, @IdStudy, @Semester + 1, GETDATE());
							
						UPDATE Student SET IdEnrollment = @NextEnrollmentId WHERE IdEnrollment = @EnrollmentId;
					
						COMMIT TRAN
						RETURN @NextEnrollmentId;
				    END TRY	
				BEGIN CATCH
					ROLLBACK TRAN
				END CATCH
			END		
	END