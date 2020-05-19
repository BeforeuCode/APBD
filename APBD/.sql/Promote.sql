CREATE PROCEDURE PromoteStudent
	@Semester int,
	@Study varchar(50)
AS
	BEGIN 
	 BEGIN TRAN
		DECLARE @EnrollmentId INT = (SELECT e.IdEnrollment FROM Enrollment e 
		INNER JOIN Studies s ON s.IdStudy = e.IdStudy
		WHERE s.Name = @Study AND e.Semester = @Semester); 

		IF @EnrollmentId IS NOT NULL
			BEGIN
				DECLARE @EnrollmentIdForUpdate INT = (SELECT e.IdEnrollment FROM Enrollment e 
				INNER JOIN Studies s ON s.IdStudy = e.IdStudy
				WHERE s.Name = @Study AND e.Semester = @Semester + 1);

					IF @EnrollmentIdForUpdate IS NOT NULL
						BEGIN
							UPDATE Student SET IdEnrollment = @EnrollmentIdForUpdate WHERE IdEnrollment = @EnrollmentId;
						    SELECT e.Semester, s.Name, e.StartDate 
                                   FROM Enrollment e 
                                   INNER JOIN Studies s ON e.IdStudy = s.IdStudy 
                                   WHERE e.IdEnrollment = @EnrollmentIdForUpdate;
						END
					ELSE 
						BEGIN
							DECLARE @NextEnrollmentId INT = (SELECT (MAX(e.IdEnrollment) + 1) FROM Enrollment e)
							DECLARE @IdStudy INT  = (SELECT s.IdStudy FROM Studies s WHERE s.Name = @Study);
						
							INSERT INTO Enrollment(IdEnrollment,IdStudy,Semester, StartDate)
							VALUES (@NextEnrollmentId, @IdStudy, @Semester + 1, GETDATE());
							
							UPDATE Student SET IdEnrollment = @NextEnrollmentId WHERE IdEnrollment = @EnrollmentId;
							SELECT e.Semester, s.Name, e.StartDate 
                                   FROM Enrollment e 
                                   INNER JOIN Studies s ON e.IdStudy = s.IdStudy 
                                   WHERE e.IdEnrollment = @NextEnrollmentId;
					END
				END
			ELSE
				BEGIN
					RAISERROR (15600,-1,-1, 'PromoteS');  
				END
		END	