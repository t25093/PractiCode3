CREATE DATABASE ToDoDB;          -- יוצר מסד נתונים חדש בשם ToDoDB

USE ToDoDB;                      -- בוחר בו כמסד הפעיל, כדי שהטבלה תיווצר בתוכו

CREATE TABLE Items (             -- יוצר טבלה בשם Items
  Id INT NOT NULL AUTO_INCREMENT,-- מזהה מספרי, חובה, גדל אוטומטית בכל הוספה
  Name VARCHAR(100),             -- שם המשימה, עד 100 תווים
  IsComplete TINYINT(1),         -- האם הושלמה: 0 = לא, 1 = כן (ייתרגם ל־bool ב־C#)
  PRIMARY KEY (Id)               -- Id הוא המפתח הראשי של הטבלה
);

INSERT INTO Items (Name, IsComplete) VALUES ('משימה ראשונה', 0), ('משימה שנייה', 1);
