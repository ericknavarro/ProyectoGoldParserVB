﻿'Generated by the GOLD Parser Builder

Option Explicit On
Option Strict Off

Imports System.IO
'----------------------------------------------------------------------------------------------
'Por error Gold parser generó este archivo con un punto y coma al final del siguiente
'comando import, el error se corrigió manualmente.
'----------------------------------------------------------------------------------------------
Imports System.Windows.Forms


Module MyParser
    Private Parser As New GOLD.Parser
    '----------------------------------------------------------------------------------------------
    'A continuación se declara el objeto Root, que será la raíz del arbol sintáctico a evaluar
    '----------------------------------------------------------------------------------------------
    Private Root As GOLD.Reduction


    Private Enum SymbolIndex
        [Eof] = 0                                 ' (EOF)
        [Error] = 1                               ' (Error)
        [Whitespace] = 2                          ' Whitespace
        [Minus] = 3                               ' '-'
        [Lparen] = 4                              ' '('
        [Rparen] = 5                              ' ')'
        [Times] = 6                               ' '*'
        [Div] = 7                                 ' '/'
        [Semi] = 8                                ' ';'
        [Lbracket] = 9                            ' '['
        [Rbracket] = 10                           ' ']'
        [Plus] = 11                               ' '+'
        [Decimal] = 12                            ' DECIMAL
        [Entero] = 13                             ' ENTERO
        [Evaluar] = 14                            ' Evaluar
        [Expression] = 15                         ' <Expression>
        [Multexp] = 16                            ' <Mult Exp>
        [Negateexp] = 17                          ' <Negate Exp>
        [Statement] = 18                          ' <Statement>
        [Statements] = 19                         ' <Statements>
        [Value] = 20                              ' <Value>
    End Enum

    Private Enum ProductionIndex
        [Statements] = 0                          ' <Statements> ::= <Statement> <Statements>
        [Statements2] = 1                         ' <Statements> ::= <Statement>
        [Statement_Evaluar_Lbracket_Rbracket_Semi] = 2 ' <Statement> ::= Evaluar '[' <Expression> ']' ';'
        [Expression_Plus] = 3                     ' <Expression> ::= <Expression> '+' <Mult Exp>
        [Expression_Minus] = 4                    ' <Expression> ::= <Expression> '-' <Mult Exp>
        [Expression] = 5                          ' <Expression> ::= <Mult Exp>
        [Multexp_Times] = 6                       ' <Mult Exp> ::= <Mult Exp> '*' <Negate Exp>
        [Multexp_Div] = 7                         ' <Mult Exp> ::= <Mult Exp> '/' <Negate Exp>
        [Multexp] = 8                             ' <Mult Exp> ::= <Negate Exp>
        [Negateexp_Minus] = 9                     ' <Negate Exp> ::= '-' <Value>
        [Negateexp] = 10                          ' <Negate Exp> ::= <Value>
        [Value_Entero] = 11                       ' <Value> ::= ENTERO
        [Value_Decimal] = 12                      ' <Value> ::= DECIMAL
        [Value_Lparen_Rparen] = 13                ' <Value> ::= '(' <Expression> ')'
    End Enum

    Public Program As Object     'You might derive a specific object

    Public Sub Setup()
        'This procedure can be called to load the parse tables. The class can
        'read tables using a BinaryReader.

        '----------------------------------------------------------------------------------------------
        'Gold Parser generó el esqueleto con el siguiente comando:
        'Parser.LoadTables(Path.Combine(Application.StartupPath, "Expresiones aritméticas.egt"))
        '----------------------------------------------------------------------------------------------
        'Para efectos de nuestra aplicación de consola, se modificará manualmente:
        '----------------------------------------------------------------------------------------------
        Parser.LoadTables(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Expresiones aritméticas.egt"))
    End Sub

    '----------------------------------------------------------------------------------------------
    'El siguiente método público fue agregado para que otras clases puedan acceder al árbol
    'generado en esta clase, este se amacenará en la variable privada Root.
    '----------------------------------------------------------------------------------------------
    Public Function GetRoot() As GOLD.Reduction
        Return Root
    End Function

    Public Function Parse(ByVal Reader As TextReader) As Boolean
        'This procedure starts the GOLD Parser Engine and handles each of the
        'messages it returns. Each time a reduction is made, you can create new
        'custom object and reassign the .CurrentReduction property. Otherwise, 
        'the system will use the Reduction object that was returned.
        '
        'The resulting tree will be a pure representation of the language 
        'and will be ready to implement.

        Dim Response As GOLD.ParseMessage
        Dim Done As Boolean                  'Controls when we leave the loop
        Dim Accepted As Boolean = False      'Was the parse successful?

        Accepted = False    'Unless the program is accepted by the parser

        Parser.Open(Reader)
        Parser.TrimReductions = False  'Please read about this feature before enabling  

        Done = False
        Do Until Done
            Response = Parser.Parse()

            Select Case Response
                Case GOLD.ParseMessage.LexicalError
                    'Cannot recognize token
                    Console.WriteLine("Error Lexico. No se reconocer el caracter " + Parser.CurrentToken.Data.ToString() + " en fila: " + Parser.CurrentToken.Position.Line.ToString() + " columna: " + Parser.CurrentToken.Position.Column.ToString())
                    Done = True

                Case GOLD.ParseMessage.SyntaxError
                    'Expecting a different token
                    Console.WriteLine("Error de Sintaxis. No se esperaba el caracter " + Parser.CurrentToken.Data.ToString() + " en fila: " + Parser.CurrentToken.Position.Line.ToString() + " columna: " + Parser.CurrentToken.Position.Column.ToString())
                    Done = True

                Case GOLD.ParseMessage.Reduction
                    'Create a customized object to store the reduction
                    '----------------------------------------------------------------------------------------------
                    'Gold Parser generó el siguiente comando con un punto al inicio, esto se corrigió manualmente
                    'comentando dicha línea
                    '----------------------------------------------------------------------------------------------
                    '.CurrentReduction = CreateNewObject(Parser.CurrentReduction)

                Case GOLD.ParseMessage.Accept
                    'Accepted!
                    'Program = Parser.CurrentReduction  'The root node!    
                    Root = Parser.CurrentReduction
                    If Root IsNot Nothing Then
                        GetValue(Root)
                    End If

                    Done = True
                    Accepted = True

                Case GOLD.ParseMessage.TokenRead
                    'You don't have to do anything here.

                Case GOLD.ParseMessage.InternalError
                    'INTERNAL ERROR! Something is horribly wrong.
                    Done = True

                Case GOLD.ParseMessage.NotLoadedError
                    'This error occurs if the CGT was not loaded.                   
                    Done = True

                Case GOLD.ParseMessage.GroupError
                    'COMMENT ERROR! Unexpected end of file
                    Done = True
            End Select
        Loop

        Return Accepted
    End Function

    Public Function GetValue(root As GOLD.Reduction) As Object
        Select Case root.Parent.TableIndex
            Case ProductionIndex.Statements
                ' <Statements> ::= <Statement> <Statements> 
                GetValue(root(0).Data)                                      ''Recorre la produccion Statement
                GetValue(root(1).Data)                                      ''Recorre la produccion Statements

            Case ProductionIndex.Statements2
                ' <Statements> ::= <Statement>      
                GetValue(root(0).Data)                                      '' Recorre la produccion Statement

            Case ProductionIndex.Statement_Evaluar_Lbracket_Rbracket_Semi
                ' <Statement> ::= Evaluar '[' <Expression> ']' ';' 
                Console.WriteLine(GetValue(root(2).Data))                   ''Imprime en consola el resultado de la Expresion

            Case ProductionIndex.Expression_Plus
                ' <Expression> ::= <Expression> '+' <Mult Exp> 
                Return GetValue(root(0).Data) + GetValue(root(2).Data)      ''Retorna la suma de los dos numeros

            Case ProductionIndex.Expression_Minus
                ' <Expression> ::= <Expression> '-' <Mult Exp> 
                Return GetValue(root(0).Data) - GetValue(root(2).Data)      ''Retorna la resta de los dos numeros

            Case ProductionIndex.Expression
                ' <Expression> ::= <Mult Exp> 
                Return GetValue(root(0).Data)

            Case ProductionIndex.Multexp_Times
                ' <Mult Exp> ::= <Mult Exp> '*' <Negate Exp> 
                Return GetValue(root(0).Data) * GetValue(root(2).Data)      ''Retorna el producto de los dos numeros

            Case ProductionIndex.Multexp_Div
                ' <Mult Exp> ::= <Mult Exp> '/' <Negate Exp> 
                Return GetValue(root(0).Data) / GetValue(root(2).Data)      ''Retorna la division de los dos numeros

            Case ProductionIndex.Multexp
                ' <Mult Exp> ::= <Negate Exp> 
                Return GetValue(root(0).Data)

            Case ProductionIndex.Negateexp_Minus
                ' <Negate Exp> ::= '-' <Value> 
                Return GetValue(root(1).Data) * -1                          ''Retorna la negacion del numero

            Case ProductionIndex.Negateexp
                ' <Negate Exp> ::= <Value> 
                Return GetValue(root(0).Data)

            Case ProductionIndex.Value_Entero
                ' <Value> ::= ENTERO 
                Return Double.Parse(root(0).Data.ToString())                ''Retorna la representacion decimal del numero

            Case ProductionIndex.Value_Decimal
                ' <Value> ::= DECIMAL 
                Return Double.Parse(root(0).Data.ToString())                ''Retorna la representacion decimal del numero

            Case ProductionIndex.Value_Lparen_Rparen
                ' <Value> ::= '(' <Expression> ')' 
                Return GetValue(root(1).Data)

        End Select
        '----------------------------------------------------------------------------------------------
        'Es importante agregar el siguiente Return que aplicará a los casos en los que no se entra
        'a ninguno de los cases.
        '----------------------------------------------------------------------------------------------
        Return Nothing
    End Function

End Module