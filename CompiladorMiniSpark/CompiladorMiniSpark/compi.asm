INCLUDE macros.mac
DOSSEG
.MODEL SMALL
STACK 100H
.DATA
			MAXLEN DB 254
			LEN DB 0
			MSG   DB 254 DUP(?)
			MSG_DD   DD MSG
			BUFFER		DB 8 DUP('$')
			CADENA_NUM		DB 10 DUP('$')
			BUFFERTEMP	DB 8 DUP('$')
			BLANCO	DB '#'
			BLANCOS	DB '$'
			MENOS	DB '-$'
			COUNT	DW 0
			NEGATIVO	DB 0
			BUF	DW 10
			LISTAPAR	LABEL BYTE
			LONGMAX	DB 254
			TRUE	DW 1
			FALSE DW 0
			INTRODUCIDOS	DB 254 DUP ('$')
			MULT10	DW 1
			s_true	DB 'true$'
			s_false DB 'false$'
			numero1  DW 0
			numero2  DW 0
			t0  DW 0
			t1  DW 0
.CODE
.386
BEGIN:
			MOV     AX, @DATA
			MOV     DS, AX
CALL COMPI
			MOV     AX, 4C00H
			INT 	21H
COMPI PROC
	I_ASIGNAR numero2, 5
READ
ASCTODEC numero1,MSG
	I_MAYOR numero1,numero2,t0
	JF t0,A0
	ITOA BUFFER, 2
	WRITE BUFFERTEMP
	JMP B0
A0:
	I_MAYOR numero2,numero1,t1
	JF t1,A1
	ITOA BUFFER, 1
	WRITE BUFFERTEMP
	JMP B1
A1:
	ITOA BUFFER, 0
	WRITE BUFFERTEMP
B1:
B0:
		ret
COMPI ENDP
END BEGIN