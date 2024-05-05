Imports System.Data.SqlClient
Imports System.Threading
Imports NUnit.Framework

Namespace TestProject1

    Public Class Tests
        Dim connectionString As String

        <SetUp>
        Public Sub Setup()
        End Sub

        Class ThreadData
            Public Property ConnectionString As String
            Public Property TableName As String
            Public Property Iterations As Integer

            Public Sub New(connectionString As String, tableName As String, iterations As Integer)
                Me.ConnectionString = connectionString
                Me.TableName = tableName
                Me.Iterations = iterations
            End Sub
        End Class

        Sub InsertData(ByVal data As Object)
            Dim threadData As ThreadData = CType(data, ThreadData)
            Dim connectionString As String = threadData.ConnectionString
            Dim tableName As String = threadData.TableName
            Dim iterations As Integer = threadData.Iterations

            For i As Integer = 1 To iterations
                ' Generate some sample data (modify as needed)
                Dim random As New Random()
                ' Generate a random integer between 1 and 100 (inclusive)
                Dim id As Integer = random.Next(1, 1000)

                ' Insert data into the database
                Using connection As New SqlConnection(connectionString)
                    Dim query As String = String.Format("INSERT INTO {0} (id) VALUES (@id)", tableName)
                    Using command As New SqlCommand(query, connection)
                        command.Parameters.AddWithValue("@id", id)
                        connection.Open()
                        command.ExecuteNonQuery()
                        connection.Close()
                    End Using
                End Using

                ' Simulate some work (optional)
                ' Thread.Sleep(10) ' Uncomment to introduce a slight delay
            Next
        End Sub

        Sub InsertData2()
            ' Generate some sample data (modify as needed)
            Dim random As New Random()
            ' Generate a random integer between 1 and 100 (inclusive)
            Dim id As Integer = random.Next(1, 1000)

            ' Insert data into the database
            Using connection As New SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=KITE;Data Source=Himawarithie;Pooling=true;Min Pool Size=10;Max Pool Size=20")
                Dim query As String = String.Format("INSERT INTO {0} (id) VALUES (@id)", "stressTest")
                Using command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@id", id)
                    connection.Open()
                    command.ExecuteNonQuery()
                    connection.Close()
                End Using
            End Using
        End Sub

        <Test>
        Public Sub Test1()
            Dim connectionString As String = "Blalalalabla;Min Pool Size=10;Max Pool Size=20" ' Replace with your actual connection string
            Dim tableName As String = "stressTest"  ' Replace with your actual table name
            Dim numberOfThreads As Integer = 10 ' Number of threads for stress test
            Dim iterationsPerThread As Integer = 200 ' Number of inserts per thread

            Dim threads = New List(Of Thread)()

            For i As Integer = 1 To numberOfThreads
                Dim thread = New Thread(AddressOf InsertData)
                thread.Start(New ThreadData(connectionString, tableName, iterationsPerThread))
                threads.Add(thread)
            Next

            ' Wait for all threads to finish
            For Each thread As Thread In threads
                thread.Join()
            Next

            Console.WriteLine("Stress test completed. Inserted data using {0} threads.", numberOfThreads)

            'InsertData2()
        End Sub

    End Class

End Namespace
