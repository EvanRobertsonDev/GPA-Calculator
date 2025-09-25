1. Basic UI Elements
	The program uses buttons, labels, textboxes, stackpanels, grid layouts, and an image

2. Populating Selection Elements
	The program contains a Combobox to allow choosing between different students

3. Collection Datatypes
	The program uses a dictionary to store the Letter Grades and their corresponding point value.
	It also uses lists for storing students and courses to easily allow adding and removing specific students/courses from the system.
	Stacks were also used for adding and removing course inputs (Coursename, Letter Grade, and Credits) to allow both adding and removing from the bottom of the collection.

4. Datagrid
	A datagrid view is present and populates after the user selects a student from the Combobox.

5. External Data and Saving to files
	Data is read from an XML file and can be created, updated, and deleted within the program.

6. Second Form
	The second form was not clear to me exactly what it should be doing. The program is split in half, one side(form) for student management, and the other(form) for couese management.
	I also have popup message boxes for when data is entered incorrectly to let the user 
	know to correct their mistakes.

7. Topics Not Covered in Class
	The program takes advantage of borders and enabling/disabling UI elements to make the flow of the program easier for the user to understand.
	For example, if a student isn't selected, the user cannot enter grades for courses.
	The borders assist in breaking up the program into different parts.

	For the Student and Course classes I also make use of the init keyword in the place of a setter, this makes specific properties read-only after the
	instantiation of the object. So I used this for the Student's ID and Name.