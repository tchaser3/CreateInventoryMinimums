'Title:         Compute Max and Min
'Date:          4-1-15
'Author:        Terry Holmes

'Description:   This program will computer a Max and Mins

Option Strict On

Public Class Form1

    'Setting up the global variables\
    Private TheWarehouseInventoryDataSet As WarehouseInventoryDataSet
    Private TheWarehouseInventoryDataTier As WarehouseInventoryDataTier
    Private WithEvents TheWarehouseInventoryBindingSource As BindingSource

    Private TheIssuedPartsDataSet As IssuedPartsDataSet
    Private TheissuedPartsDataTier As IssuedPartsDataTier
    Private WithEvents TheIssuedPartsBindingSource As BindingSource

    Private TheEmployeeDataSet As EmployeesDataSet
    Private TheEmployeeDataTier As EmployeeDataTier
    Private WithEvents TheEmployeeBindingSource As BindingSource

    Private ThePartNumberDataSet As PartNumberDataSet
    Private ThePartNumberDataTier As PartNumberDataTier
    Private WithEvents ThePartNumberBindingSource As BindingSource

    Private addingBoolean As Boolean = False
    Private editingBoolean As Boolean = False
    Private previousSelectedIndex As Integer

    Structure WarehouseInventory
        Dim mintSelectedIndex As Integer
        Dim mintPartID As Integer
        Dim mstrPartNumber As String
        Dim mintQuantityOnHand As Integer
        Dim mintWarehouseID As Integer
        Dim mintAverageQuantity As Integer
        Dim mintMin As Integer
        Dim mintMax As Integer
    End Structure

    Structure IssueTransactions
        Dim mstrPartNumber As String
        Dim mintQuantity As Integer
    End Structure

    Dim structWarehouseInventory() As WarehouseInventory
    Dim structIssuedTransaction() As IssueTransactions
    Dim mintStructureCounter As Integer
    Dim mintStructureUpperLimit As Integer
    Dim mintIssuedCounter As Integer
    Dim mintIssuedUpperLimit As Integer

    Dim mintEmployeeSelectedIndex() As Integer
    Dim mintEmployeeCounter As Integer
    Dim mintEmployeeUpperLimit As Integer
    Dim mintNewPrintCounter As Integer
    Dim logDate As Date = Date.Now
    Dim mstrDate As String

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        'This will close the program
        CloseProgram.ShowDialog()

    End Sub
    Private Sub SetWarehouseInventoryDataBindings()

        'This will binding the controls
        'Setting local variable
        Dim intNumberOfRecords As Integer

        Try

            'setting up the data control
            TheWarehouseInventoryDataTier = New WarehouseInventoryDataTier
            TheWarehouseInventoryDataSet = TheWarehouseInventoryDataTier.GetWarehouseInventoryInformation
            TheWarehouseInventoryBindingSource = New BindingSource

            'setting the binding source
            With TheWarehouseInventoryBindingSource
                .DataSource = TheWarehouseInventoryDataSet
                .DataMember = "WarehouseInventory"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboWHPartID
                .DataSource = TheWarehouseInventoryBindingSource
                .DisplayMember = "PartID"
                .DataBindings.Add("text", TheWarehouseInventoryBindingSource, "PartID", False, DataSourceUpdateMode.Never)
            End With

            'Setting the rest of the controls
            txtWHPartNumber.DataBindings.Add("text", TheWarehouseInventoryBindingSource, "PartNumber")
            txtWHQuantity.DataBindings.Add("text", TheWarehouseInventoryBindingSource, "QTYOnHand")
            txtWHWarehouseID.DataBindings.Add("text", TheWarehouseInventoryBindingSource, "WarehouseID")
            txtWHMin.DataBindings.Add("text", TheWarehouseInventoryBindingSource, "Min")
            txtWHMax.DataBindings.Add("text", TheWarehouseInventoryBindingSource, "Max")

            intNumberOfRecords = cboWHPartID.Items.Count - 1
            ReDim structWarehouseInventory(intNumberOfRecords)

        Catch ex As Exception

            'Message to user
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub
    Private Sub SetIssuedPartsDataBindings()

        'Try Catch for exception
        Try

            'Setting up the control variables
            TheissuedPartsDataTier = New IssuedPartsDataTier
            TheIssuedPartsDataSet = TheissuedPartsDataTier.GetIssuedPartsInformation
            TheIssuedPartsBindingSource = New BindingSource

            'Setting up the binding source
            With TheIssuedPartsBindingSource
                .DataSource = TheIssuedPartsDataSet
                .DataMember = "IssuedParts"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboIssuedTransactionID
                .DataSource = TheIssuedPartsBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("Text", TheIssuedPartsBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'setting up the rest of the controls
            txtIssuedPartNumber.DataBindings.Add("text", TheIssuedPartsBindingSource, "PartNumber")
            txtIssuedQuantity.DataBindings.Add("text", TheIssuedPartsBindingSource, "QTY")
            txtIssuedDate.DataBindings.Add("Text", TheIssuedPartsBindingSource, "Date")

        Catch ex As Exception

            'Message to user
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub
    Private Sub SetEmployeeDataBindings()

        'Setting local variables
        Dim intCounter As Integer
        Dim intNumberOfRecords As Integer
        Dim strLastNameForSearch As String
        Dim strLastNameFromTable As String

        'try catch to bind controls
        Try

            'Setting the data variables
            TheEmployeeDataTier = New EmployeeDataTier
            TheEmployeeDataSet = TheEmployeeDataTier.GetEmployeesInformation
            TheEmployeeBindingSource = New BindingSource

            'Setting up the binding source
            With TheEmployeeBindingSource
                .DataSource = TheEmployeeDataSet
                .DataMember = "employees"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboEmployeeID
                .DataSource = TheEmployeeBindingSource
                .DisplayMember = "EmployeeID"
                .DataBindings.Add("text", TheEmployeeBindingSource, "EmployeeID", False, DataSourceUpdateMode.Never)
            End With

            'Setting up the rest of the controls
            txtFirstName.DataBindings.Add("Text", TheEmployeeBindingSource, "FirstName")
            txtLastName.DataBindings.Add("text", TheEmployeeBindingSource, "LastName")

            intNumberOfRecords = cboEmployeeID.Items.Count - 1
            strLastNameForSearch = "PARTS"
            ReDim mintEmployeeSelectedIndex(intNumberOfRecords)
            mintEmployeeCounter = 0

            For intCounter = 0 To intNumberOfRecords

                'incrementing the combo box
                cboEmployeeID.SelectedIndex = intCounter

                'getting the last name
                strLastNameFromTable = txtLastName.Text

                If strLastNameForSearch = strLastNameFromTable Then
                    mintEmployeeSelectedIndex(mintEmployeeCounter) = intCounter
                    mintEmployeeCounter += 1
                End If
            Next

            'Setting up the navigation controls
            mintEmployeeUpperLimit = mintEmployeeCounter - 1
            mintEmployeeCounter = 0
            If mintEmployeeUpperLimit > 0 Then
                btnNext.Enabled = True
            End If
            cboEmployeeID.SelectedIndex = mintEmployeeSelectedIndex(0)
            btnSelect.Enabled = True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SetPartNumberDataBindings()

        'Try catch for exeptions
        Try

            'Setting up the data variables
            ThePartNumberDataTier = New PartNumberDataTier
            ThePartNumberDataSet = ThePartNumberDataTier.GetPartNumberInformation
            ThePartNumberBindingSource = New BindingSource

            'Setting up the binding source
            With ThePartNumberBindingSource
                .DataSource = ThePartNumberDataSet
                .DataMember = "partnumbers"
                .MoveFirst()
                .MoveLast()
            End With

            'setting up the combo box
            With cboPartID
                .DataSource = ThePartNumberBindingSource
                .DisplayMember = "PartID"
                .DataBindings.Add("text", ThePartNumberBindingSource, "PartID", False, DataSourceUpdateMode.Never)
            End With

            'Setting up the rest of the controls
            txtPartNumber.DataBindings.Add("text", ThePartNumberBindingSource, "PartNumber")
            txtDescription.DataBindings.Add("text", ThePartNumberBindingSource, "Description")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'This will see the controls
        SetWarehouseInventoryDataBindings()
        SetIssuedPartsDataBindings()
        SetEmployeeDataBindings()
        SetPartNumberDataBindings()
        SetControlsVisible(False)

    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click

        'This will increment the navigation controls
        mintEmployeeCounter += 1
        cboEmployeeID.SelectedIndex = mintEmployeeSelectedIndex(mintEmployeeCounter)

        btnBack.Enabled = True

        If mintEmployeeCounter = mintEmployeeUpperLimit Then
            btnNext.Enabled = False
        End If
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click

        'This will decrement the navigation controls
        mintEmployeeCounter -= 1
        cboEmployeeID.SelectedIndex = mintEmployeeSelectedIndex(mintEmployeeCounter)

        btnNext.Enabled = True

        If mintEmployeeCounter = 0 Then
            btnBack.Enabled = False
        End If
    End Sub
    Private Sub SetControlsVisible(ByVal valueBoolean As Boolean)

        'setting the controls visible
        cboIssuedTransactionID.Visible = valueBoolean
        cboPartID.Visible = valueBoolean
        cboWHPartID.Visible = valueBoolean
        txtIssuedPartNumber.Visible = valueBoolean
        txtIssuedQuantity.Visible = valueBoolean
        txtPartNumber.Visible = valueBoolean
        txtWHMax.Visible = valueBoolean
        txtWHMin.Visible = valueBoolean
        txtWHPartNumber.Visible = valueBoolean
        txtWHQuantity.Visible = Visible
        txtWHWarehouseID.Visible = valueBoolean
        txtWHQuantity.Visible = valueBoolean
        txtDescription.Visible = valueBoolean

    End Sub
    Private Sub LoadIssuedStructure()

        'Setting local variables
        Dim intCounter As Integer
        Dim intNumberOfRecords As Integer
        Dim datDateForSearch As Date
        Dim datDateFromTable As Date
        Dim strDate As String

        strDate = "01/01/2014"
        intNumberOfRecords = cboIssuedTransactionID.Items.Count - 1
        datDateForSearch = CDate(strDate)
        mintIssuedCounter = 0

        'Beginning Loop
        For intCounter = 0 To intNumberOfRecords

            'incrementing the combo box
            cboIssuedTransactionID.SelectedIndex = intCounter

            'Getting the date
            datDateFromTable = CDate(txtIssuedDate.Text)

            If datDateFromTable > datDateForSearch Then

                structIssuedTransaction(mintIssuedCounter).mstrPartNumber = txtIssuedPartNumber.Text
                structIssuedTransaction(mintIssuedCounter).mintQuantity = CInt(txtIssuedQuantity.Text)
                mintIssuedCounter += 1
            End If
        Next

        mintIssuedUpperLimit = mintIssuedCounter - 1

    End Sub

    Private Sub btnSelect_Click(sender As Object, e As EventArgs) Handles btnSelect.Click

        'This will activate the controls and load the structure
        'Setting local variables
        Dim intCounter As Integer
        Dim intNumberOfRecords As Integer
        Dim intWarehouseIDForSearch As Integer
        Dim intWarehouseIDFromTable As Integer

        'Setting the controls visible
        SetControlsVisible(True)

        'Getting ready to load the structure
        intNumberOfRecords = cboWHPartID.Items.Count - 1
        mintStructureCounter = 0
        mintStructureUpperLimit = 0
        intWarehouseIDForSearch = CInt(cboEmployeeID.Text)

        'Performing loop
        For intCounter = 0 To intNumberOfRecords

            'incrementing the combo box
            cboWHPartID.SelectedIndex = intCounter

            'Getting the warehouse id
            intWarehouseIDFromTable = CInt(txtWHWarehouseID.Text)

            If intWarehouseIDForSearch = intWarehouseIDFromTable Then

                'Loading up the structure
                structWarehouseInventory(mintStructureCounter).mintSelectedIndex = cboWHPartID.SelectedIndex
                structWarehouseInventory(mintStructureCounter).mintPartID = CInt(cboWHPartID.Text)
                structWarehouseInventory(mintStructureCounter).mintWarehouseID = CInt(txtWHWarehouseID.Text)
                structWarehouseInventory(mintStructureCounter).mintQuantityOnHand = CInt(txtWHQuantity.Text)
                structWarehouseInventory(mintStructureCounter).mstrPartNumber = txtWHPartNumber.Text
                mintStructureCounter += 1

            End If
        Next

        intNumberOfRecords = cboIssuedTransactionID.Items.Count - 1
        ReDim structIssuedTransaction(intNumberOfRecords)

        'Setting the limit
        mintStructureUpperLimit = mintStructureCounter - 1
        LoadIssuedStructure()
        btnProcess.Enabled = True
        btnNext.Enabled = False
        btnBack.Enabled = False
        btnSelect.Enabled = False

    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click

        'This will process the transactions and come up with a Min and Max
        'setting local variables
        Dim intInventoryCounter As Integer
        Dim intInventoryQuantity As Integer
        Dim intTransactionCounter As Integer
        Dim strPartNumberForSearch As String
        Dim strPartNumberFromTable As String
        Dim intQuantityIssued As Integer
        Dim intQuantityIssuedTotal As Integer
        Dim decQuantityAverage As Decimal
        Dim decMax As Decimal
        Dim decMin As Decimal

        PleaseWait.show()
        
        'Beginning Loop
        For intInventoryCounter = 0 To mintStructureUpperLimit

            'Setting value for the quantity
            intQuantityIssuedTotal = 0
            intInventoryQuantity = structWarehouseInventory(intInventoryCounter).mintQuantityOnHand

            'Getting part Number
            strPartNumberForSearch = structWarehouseInventory(intInventoryCounter).mstrPartNumber

            'Looping the transactions
            For intTransactionCounter = 0 To mintIssuedUpperLimit

                'Getting the partnumber
                strPartNumberFromTable = structIssuedTransaction(intTransactionCounter).mstrPartNumber
                
                    If strPartNumberForSearch = strPartNumberFromTable Then

                    intQuantityIssued = structIssuedTransaction(intTransactionCounter).mintQuantity

                        If intQuantityIssued < 0 Then
                            intQuantityIssued = 0
                        End If

                        intQuantityIssuedTotal = intQuantityIssuedTotal + intQuantityIssued
                    End If
            Next

            decQuantityAverage = CDec(intQuantityIssuedTotal / 9)
            decMax = decQuantityAverage + decQuantityAverage / 2 + decQuantityAverage / 10
            decMin = decQuantityAverage + decQuantityAverage / 5

            structWarehouseInventory(intInventoryCounter).mintAverageQuantity = CInt(Decimal.Truncate(decQuantityAverage))
            structWarehouseInventory(intInventoryCounter).mintMax = CInt(Decimal.Truncate(decMax))
            structWarehouseInventory(intInventoryCounter).mintMin = CInt(Decimal.Truncate(decMin))

            cboWHPartID.SelectedIndex = structWarehouseInventory(intInventoryCounter).mintSelectedIndex

            txtWHMax.Text = CStr(structWarehouseInventory(intInventoryCounter).mintMax)
            txtWHMin.Text = CStr(structWarehouseInventory(intInventoryCounter).mintMin)
            Try
                TheWarehouseInventoryBindingSource.EndEdit()
                TheWarehouseInventoryDataTier.UpdateWarehouseInventoryDB(TheWarehouseInventoryDataSet)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        Next

        PleaseWait.Close()

        If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        End If

        'Getting ready to print
        mintNewPrintCounter = 0
        PrintDocument1.Print()

    End Sub
    Private Sub FindPartNumber(ByVal strPartNumberForSearch As String)

        Dim intCounter As Integer
        Dim intNumberOfRecords As Integer
        Dim intSelectedIndex As Integer
        Dim strPartNumberFromTable As String

        'getting ready for the loop
        intNumberOfRecords = cboPartID.Items.Count - 1

        'running loop
        For intCounter = 0 To intNumberOfRecords

            'incrementing the combo box
            cboPartID.SelectedIndex = intCounter

            'Getting the part number
            strPartNumberFromTable = txtPartNumber.Text

            If strPartNumberForSearch = strPartNumberFromTable Then
                intSelectedIndex = intCounter
            End If
        Next

        cboPartID.SelectedIndex = intSelectedIndex

    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        'Setting local variables
        Dim intCounter As Integer
        Dim intStartingPageCounter As Integer
        Dim blnNewPage As Boolean = False

        'Setting up variables for the reports
        Dim PrintHeaderFont As New Font("Arial", 18, FontStyle.Bold)
        Dim PrintSubHeaderFont As New Font("Arial", 14, FontStyle.Bold)
        Dim PrintItemsFont As New Font("Arial", 10, FontStyle.Regular)
        Dim PrintX As Single = e.MarginBounds.Left
        Dim PrintY As Single = e.MarginBounds.Top
        Dim HeadingLineHeight As Single = PrintHeaderFont.GetHeight + 18
        Dim ItemLineHeight As Single = PrintItemsFont.GetHeight + 10

        'Variables for reducing the part discription
        Dim intCharacterCount As Integer
        Dim intCharacterLimit As Integer = 25
        Dim chaCharacterDescription() As Char
        Dim intStringLength As Integer
        Dim strPartDescription As String = ""

        'Getting the date
        mstrDate = CStr(LogDate)

        'Setting up for default position
        PrintY = 100

        'Setting up for the print header
        PrintX = 150
        e.Graphics.DrawString("Blue Jay Communications Inventory Report", PrintHeaderFont, Brushes.Black, PrintX, PrintY)
        PrintY = PrintY + HeadingLineHeight
        PrintX = 162
        e.Graphics.DrawString("Inventory Maximum and Minimum Report", PrintSubHeaderFont, Brushes.Black, PrintX, PrintY)
        PrintY = PrintY + HeadingLineHeight

        'Setting up the columns
        PrintX = 100
        e.Graphics.DrawString("Part Number", PrintItemsFont, Brushes.Black, PrintX, PrintY)
        PrintX = 250
        e.Graphics.DrawString("Part Description", PrintItemsFont, Brushes.Black, PrintX, PrintY)
        PrintX = 500
        e.Graphics.DrawString("Avg Issued", PrintItemsFont, Brushes.Black, PrintX, PrintY)
        PrintX = 600
        e.Graphics.DrawString("Max", PrintItemsFont, Brushes.Black, PrintX, PrintY)
        PrintX = 700
        e.Graphics.DrawString("Min", PrintItemsFont, Brushes.Black, PrintX, PrintY)
        PrintY = PrintY + HeadingLineHeight

        For intCounter = mintNewPrintCounter To mintStructureUpperLimit

            If structWarehouseInventory(intCounter).mintAverageQuantity > 0 Then

                PrintX = 100
                e.Graphics.DrawString(structWarehouseInventory(intCounter).mstrPartNumber, PrintItemsFont, Brushes.Black, PrintX, PrintY)
                FindPartNumber(structWarehouseInventory(intCounter).mstrPartNumber)
                PrintX = 250

                'Setting up to limit the description size
                strPartDescription = txtDescription.Text
                intStringLength = strPartDescription.Length

                If intStringLength > intCharacterLimit Then

                    'Setting up for the loop
                    chaCharacterDescription = strPartDescription.ToCharArray
                    strPartDescription = ""

                    'Performing the loop
                    For intCharacterCount = 0 To intCharacterLimit

                        'loading up the string
                        strPartDescription = strPartDescription + CStr(chaCharacterDescription(intCharacterCount))

                    Next


                End If
                e.Graphics.DrawString(strPartDescription, PrintItemsFont, Brushes.Black, PrintX, PrintY)
                PrintX = 500
                e.Graphics.DrawString(CStr(structWarehouseInventory(intCounter).mintAverageQuantity), PrintItemsFont, Brushes.Black, PrintX, PrintY)
                PrintX = 600
                e.Graphics.DrawString(CStr(structWarehouseInventory(intCounter).mintMax), PrintItemsFont, Brushes.Black, PrintX, PrintY)
                PrintX = 700
                e.Graphics.DrawString(CStr(structWarehouseInventory(intCounter).mintMin), PrintItemsFont, Brushes.Black, PrintX, PrintY)
                PrintY = PrintY + ItemLineHeight

            End If

            If PrintY > 900 Then
                If intStartingPageCounter = mintStructureUpperLimit Then
                    e.HasMorePages = False
                Else
                    e.HasMorePages = True
                    blnNewPage = True
                End If
            End If

            If blnNewPage = True Then
                mintNewPrintCounter = intCounter + 1
                intCounter = mintStructureUpperLimit
            End If
        Next

    End Sub
End Class
