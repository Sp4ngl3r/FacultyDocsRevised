using 	UnityEngine;
using 	System.Collections;
using 	System.IO;
using	sharpPDF;
using	sharpPDF.Enumerators;



public class SimplePDF : MonoBehaviour {
	
	internal	string		attacName	= "ADMIN Summary Sheet.pdf";

	// Use this for initialization
     IEnumerator Start () {
		yield return StartCoroutine ( CreatePDF() );
	}
	
	// Update is called once per frame
	public IEnumerator CreatePDF () {
		pdfDocument myDoc = new pdfDocument("Sample Application","Me", false);
		pdfPage myFirstPage = myDoc.addPage();
        pdfPage mySecondPage = myDoc.addPage();


        Debug.Log("Continue to create PDF");
        myFirstPage.addText("ADMIN Summary Sheet",10,730,predefinedFont.csHelveticaOblique,30,new pdfColor(predefinedColor.csOrange));
		
		
		
		/*Table's creation*/
		pdfTable myTable = new pdfTable();
		//Set table's border
		myTable.borderSize = 1;
		myTable.borderColor = new pdfColor(predefinedColor.csDarkBlue);
		
		/*Add Columns to a grid*/
		myTable.tableHeader.addColumn(new pdfTableColumn("faculty name", predefinedAlignment.csLeft,90));
		myTable.tableHeader.addColumn(new pdfTableColumn("year", predefinedAlignment.csLeft,90));
		myTable.tableHeader.addColumn(new pdfTableColumn("subject", predefinedAlignment.csLeft,90));
		myTable.tableHeader.addColumn(new pdfTableColumn("section", predefinedAlignment.csLeft,90));
        myTable.tableHeader.addColumn(new pdfTableColumn("category name", predefinedAlignment.csLeft, 90));
        myTable.tableHeader.addColumn(new pdfTableColumn("filename", predefinedAlignment.csLeft, 90));
        myTable.tableHeader.addColumn(new pdfTableColumn("extension", predefinedAlignment.csLeft, 50));





        pdfTableRow myRow = myTable.createRow();
		myRow[0].columnValue = "preeti";
		myRow[1].columnValue = "2017-18";
		myRow[2].columnValue = "4-Linux System Programming";
		myRow[3].columnValue = "A";
        myRow[4].columnValue = "Ethics";
        myRow[5].columnValue = "facnames(1).pdf";
        myRow[6].columnValue = "pdf";


        myTable.addRow(myRow);
		
		//pdfTableRow myRow1 = myTable.createRow();
		//myRow1[0].columnValue = "B";
		//myRow1[1].columnValue = "130 km/h";
		//myRow1[2].columnValue = "150Kg";
		//myRow1[3].columnValue = "Yellow";
		
		//myTable.addRow(myRow1);
		
		

		/*Set Header's Style*/
		myTable.tableHeaderStyle = new pdfTableRowStyle(predefinedFont.csCourierBoldOblique,12,new pdfColor(predefinedColor.csBlack),new pdfColor(predefinedColor.csLightOrange));
		/*Set Row's Style*/
		myTable.rowStyle = new pdfTableRowStyle(predefinedFont.csCourier,8,new pdfColor(predefinedColor.csBlack),new pdfColor(predefinedColor.csWhite));
		/*Set Alternate Row's Style*/
		myTable.alternateRowStyle = new pdfTableRowStyle(predefinedFont.csCourier,8,new pdfColor(predefinedColor.csBlack),new pdfColor(predefinedColor.csLightYellow));
		/*Set Cellpadding*/
		myTable.cellpadding = 10;
		/*Put the table on the page object*/
		myFirstPage.addTable(myTable, 5, 700);

        //yield return StartCoroutine(myFirstPage.newAddImage("D:/TEMPORARY/UnitySharpPDF/picture2.jpg", 2, 100));
        //yield return StartCoroutine(mySecondPage.newAddImage("D:/TEMPORARY/UnitySharpPDF/picture1.jpg", 2, 100));
        yield return myFirstPage;
        yield return mySecondPage;
        myDoc.createPDF(attacName);
        Debug.Log(" pdf created ");
		myTable = null;

        //yield return null;
	}
}
