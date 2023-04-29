using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // En esta parte se colocó el nombre que mostrará como Título la aplicación de consola
            Console.Title = "Coin Machine";
            // La siguiente lineá se utilizó para que la consola procesara los íconos basados en el código ASCII
            Console.OutputEncoding = Encoding.UTF8;
            /*En esta parte se declararon todas las variables de necesidad global para poderlas llamar en cualquier
              parte del código*/
            string Nombre, ID, Nacionalidad;
            double monto, ganaciaacumulada = 0;
            string Ntarjeta;
            int mest, añot;
            string[] SIMBOLOS = {"♣", "♦", "☺", "☼", "⌂", "💣" };
            DateTime FECHANACIMIENTO;
            //Se declaró la sección menú la cual contiene todo el sistema para poderlo llamar de nuevo.
            MENU:
            Console.Clear();
            Console.WriteLine("\tCOIN MACHINE");
            Console.Write("\n1. Iniciar Juego\n2. Salir\n\nIngrese Número de opción: ");

            //El siguiente switch lee directamente la opcion inicial del sistema
            switch (Console.ReadLine())
            {
                //El case "1" inicia el juego
                case "1":
                //Ya que se puede jugar varias veces el juego es necesario resetear algunas variables a valor 0
                ganaciaacumulada = 0;

                /*Para el ingreso de los datos personales y corroborando la validación de errores se hizo uso de 
                 un try-cath en el caso de que el usuario ingrese un dato no válido, de igual manera se le idica
                 al usuario la manera correcta en la que debe ingresar su fecha de nacimiento*/ 
                INGRESARDATOS:
                    try
                    {
                        Console.Clear();
                        Console.Write("\tDatos Personales\n\nIngrese su nombre: ");
                        Nombre = Console.ReadLine();
                        Console.Write("Ingrese su número de identificación: ");
                        ID = Console.ReadLine();
                        Console.Write("Ingrese su fecha de nacimiento en formato DD/MM/YYYY (EJ: 01/01/2000): ");
                        FECHANACIMIENTO = DateTime.Parse(Console.ReadLine());
                        Console.Write("Ingrese su nacionalidad: ");
                        Nacionalidad = Console.ReadLine();
                    }
                    //En el catch se muestra el error en color rojo y regresa a la solicitud de INGRESARDATOS
                    catch
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nUsted ha ingresado un dato no válido");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ReadKey();
                        goto INGRESARDATOS;
                    }

                    /*Debido a que el jugador unicamente puede jugar si es mayor de 21 años se hizo una resta de
                      año actual menos año de nacimiento del usuario*/
                    int MAYOR21 = DateTime.Now.Year - FECHANACIMIENTO.Year;

                    /*Ya que la resta anterior genera un dato erroneo en el caso de que el jugador aún no haya cumplido 21 años
                      durante el año en curso es necesario hacer esa verificación haciendo uso del dia y mes de nacimiento del 
                      usuario. En el caso que el usuario no cumpla con el mínimo de edad el programa le notifica que es menor 
                      a 21 años y lo redirije al menú principal*/
                    if (DateTime.Now.Month == FECHANACIMIENTO.Month && DateTime.Now.Day < FECHANACIMIENTO.Day && MAYOR21 == 21 || DateTime.Now.Month < FECHANACIMIENTO.Month && MAYOR21 == 21 || MAYOR21 < 21)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("\nUsted no puede realizar una apuesta debido a que su edad es menor a 21 años");
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ReadKey();
                        goto MENU;
                    }

                    /*De igual manera al caso anterior, para los mayores de 21 años que aún no hayan cumplido años en lo que va del
                      año en curso, se le resta 1 a los años ya que al final es necesario mostrar la edad del jugador. Por ejemplo si
                      el usuario nació el 05/05/2000, y la fecha actual es 29/04/2023, al momento de restar los años la variable
                      MAYOR21 tendrá un valor de 23, pero el usuario de momento tiene 22 años, por lo que se hace la verificación
                      siguiente para arreglar ese valor*/
                    if (DateTime.Now.Month == FECHANACIMIENTO.Month && DateTime.Now.Day < FECHANACIMIENTO.Day && MAYOR21 > 21 || DateTime.Now.Month < FECHANACIMIENTO.Month && MAYOR21 > 21)
                    {
                        MAYOR21 = MAYOR21 - 1;
                    }

                    /*Las probabilidades siguientes se determinaron a partir de los resultados de 10,000 partidas. Para obtener valores
                      lo más certeros posibles*/
                    Console.WriteLine("\n------------------------------------------------------------");
                    Console.WriteLine("|                  Probabilidades del juego                |");
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("| Probabilidad de obtener el 1000% de la apuesta |  0.11%  |");
                    Console.WriteLine("| Probabilidad de duplicar la apuesta            |  4.04%  |");
                    Console.WriteLine("| Probabilidad de obtener de regreso su apuesta  |  2.63%  |");
                    Console.WriteLine("| Probabilidad de perder la mitad de su apuesta  |  1.74%  |");
                    Console.WriteLine("| Probabilidad de perder toda su apuesta         |  1.11%  |");
                    Console.WriteLine("| Probabilidad de obtener una bomba              | 51.81%  |");
                    Console.WriteLine("------------------------------------------------------------");

                    /*El usuario debe confirmar que está de acuerdo a continuar con el juego luego de que las probabilidades
                      se le fueron mostradas, en el caso de querer continuar se le traslada a solicitar la forma de pago, sino
                      desea continuar lo regresa al menú principal, y de igual manera se hace una validación de datos, en el caso
                      de que ingrese alguna opción no válida*/
                CONTINUAR1:
                    Console.Write("\nDesea Continuar (1.Si / 2.No)\nIngrese número de opción : ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            goto FORMADEPAGO;
                        case "2":
                            goto MENU;
                        default:
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("\nUsted ha ingresado un dato no válido");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ReadKey();
                            goto CONTINUAR1;
                    }

                    /*En la sección FORMADEPAGO se le consulta al usuario si su pago será con Efectivo o Tarjeta, se hace una validación
                      en el caso el usuario no ingrese una opcion disponible; si el pago es en efectivo unicamente se le pide el monto
                      de la apuesta, si el pago es con tarjeta se pide el monto y los datos de la tarjeta.*/
                FORMADEPAGO:
                    double gananciatotal = 0;
                    Console.Clear();
                    Console.Write("\tForma de pago\n\n1. Efectivo\n2. Tarjeta de crédito o débito\n\nIngrese número de opción: ");
                    switch (Console.ReadLine())
                    {
                        /*El case "1" procesa el pago en efectivo*/
                        case "1":
                            Console.Clear();
                            Console.WriteLine("\tPago con efectivo");
                            try
                            {
                                Console.Write("\nIngrese monto a apostar: Q");
                                monto = double.Parse(Console.ReadLine());
                            }
                            catch
                            {
                                goto FORMADEPAGO;
                            }
                            break;
                        //El case 2 realiza el proceso del pago con tarjeta
                        case "2":
                            try
                            {
                                Console.Clear();
                                Console.WriteLine("\tPago con Tarjeta");
                                Console.Write("\nIngrese monto a apostar: Q");
                                monto = double.Parse(Console.ReadLine());
                                Console.Write("\nIngrese número de tarjeta: ");
                                Ntarjeta = Console.ReadLine();

                                //Se hace una validación que la tarjeta cuenta con el mínimo de 16 dítigos
                                if (Ntarjeta.Length != 16)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("\nNúmero de tarjeta no válido");
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.ReadKey();
                                    goto FORMADEPAGO;
                                }

                                Console.Write("Ingrese mes de expiración (MM): ");
                                mest = int.Parse(Console.ReadLine());
                                Console.Write("Ingrese año de expiración (YYYY): ");
                                añot = int.Parse(Console.ReadLine());

                                /*Se añadió una validación para verificar si la tarjeta ya está vencida haciendo uso del
                                  mes y año de expiración*/
                                if (añot < DateTime.Now.Year || añot == DateTime.Now.Year && mest < DateTime.Now.Month)
                                {
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("\nTarjeta Vencida");
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.ReadKey();
                                    goto FORMADEPAGO;
                                }
                            }
                            catch
                            {
                                goto FORMADEPAGO;
                            }
                            break;
                        default:
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Usted ha ingresado un dato no válido");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ReadKey();
                            goto FORMADEPAGO;
                    }
                    /*Se declaró la variable de montoinicial y se igualó con la variable monto, esto debido que el usuario puede
                      volver a apostar sus ganancias, por lo que ese monto cambia y al final es necesario tener el monto inical
                      para poder determinar de manera correcta los impuestos los cuales se basan unicamente sobre las ganancias*/
                    double montoinicial = monto;

                /*Nuevamente se le consulta al usuario si desea continuar, en el caso de querer continuar lo traslada directamente al
                  juego, mientras que sino desea continuar lo traslada al menú principal*/
                CONTINUAR2:
                    Console.Write("\nEstá seguro que desea continuar (1.Si / 2.No)\nIngrese número de opción : ");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            goto JUEGO;
                        case "2":
                            goto MENU;
                        default:
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("\nUsted ha ingresado un dato no válido");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ReadKey();
                            goto CONTINUAR2;
                    }

                    /*La sección de JUEGO realiza el proceso de generar los iconos al azar y mostrar el resultado de su apuesta*/
                JUEGO:
                    Console.WriteLine("\tResultado\n");
                    /*Se declararon unos contadores para determinar cuales iconos se mostraron y así determinar las ganancias del
                      usuario, se declaran en esta parte con valor 0 ya que puede existir un nuevo jugador o que el usuario vuelva 
                      a apostar sus gananicas*/
                    double trebol = 0, diamante = 0, cara = 0, sol = 0, bomba = 0;
                    string[,] RESULTADO = new string[1, 4];
                    string R1, R2 = "";
                    //Se declaró la funcion de Random para generar una posicion al azar del vector que contiene los simbolos.
                    Random random = new Random();
                    Console.Clear();
                    Console.WriteLine("\t   COIN MACHINE\n");
                    Console.WriteLine("\t-----------------");

                    /*En esta parte se añade a la matriz de resultado los iconos al azar*/
                    for (int x = 0; x < RESULTADO.GetLength(1); x++)
                    {
                        /*El vector que contiene los 6 iconos, estan almacenados desde la posición 0 a la 5, es por ello que el Random
                          se declaró entre (0, 6) ya que de esta manera unicamente genera valores al azar los cuales pueden ser:
                          0, 1, 2, 3, 4 y 5*/
                        int num = random.Next(0, 6);
                        /*Al momento que num ya contiene el numeró al azar, se iguala la matriz con la posicion generada al azar
                          del vector que contiene los simbolos*/
                        RESULTADO[0, x] = SIMBOLOS[num];
                        /*En dos cadenas de texto se fueron concatenando los símbolos*/
                        R1 = "| " + RESULTADO[0, x] + " ";
                        R2 += R1;
                    }
                    /*En esta parte se muestra la matriz de los resultados*/
                    Console.WriteLine("\t"+R2 + "|");
                    Console.WriteLine("\t-----------------");


                    /*Los siguientes "for" recorren toda la matriz, comparando que icono contiene en cada posicion, y cada icono
                      tiene su respectivo contador para poder determinar las ganancias finales*/
                    for (int x = 0; x < RESULTADO.GetLength(1); x++)
                    {
                        if (RESULTADO[0, x] == SIMBOLOS[0])
                        {
                            trebol += 1;
                        }
                    }
                    for (int x = 0; x < RESULTADO.GetLength(1); x++)
                    {
                        if (RESULTADO[0, x] == SIMBOLOS[1])
                        {
                            diamante += 1;
                        }
                    }
                    for (int x = 0; x < RESULTADO.GetLength(1); x++)
                    {
                        if (RESULTADO[0, x] == SIMBOLOS[2])
                        {
                            cara += 1;
                        }
                    }
                    for (int x = 0; x < RESULTADO.GetLength(1); x++)
                    {
                        if (RESULTADO[0, x] == SIMBOLOS[3])
                        {
                            sol += 1;
                        }
                    }
                    for (int x = 0; x < RESULTADO.GetLength(1); x++)
                    {
                        if (RESULTADO[0, x] == SIMBOLOS[5])
                        {
                            bomba += 1;
                        }
                    }

                    /*Al momento de ya tener la cantidad de iconos del resultado se determinaron las ganancias*/

                    /*Para el primer if se hace la verificación que no haya salido ninguna bomba y que la cantidad de diamantes
                      mostrados sea menor a 4, ya que los diamantes unicamente funciona si los 4 iconos mostrados son 4*/
                    if (bomba == 0 && diamante < 4)
                    {
                        gananciatotal = monto + (monto * trebol * 2) + (monto * cara) + (monto * (sol / 4));
                    }

                    /*Este if determina las ganancias en el caso de obtener 4 diamantes*/
                    if (diamante == 4)
                    {
                        gananciatotal = monto + (monto * 10);
                    }

                    /*Ya que si el usuario obtiene minimo una bomba, pierde todo su dinero por lo que la siguiente verificación
                      se hace en base de que al tener minimo 1 bomba pierde todo su dinero*/
                    if (bomba > 0)
                    {
                        gananciatotal = 0;
                    }

                    /*La variable ganaciaacumulada se iguala a la gananciatotal ya que existe la posibilidad de que el usuario
                      desee apostar nuevamente sus ganancias*/
                    ganaciaacumulada = gananciatotal;

                    /*En esta sección se muestra el resultado de su apuesta*/
                    Console.WriteLine("\nEl total ganado es de: Q" + gananciatotal);

                    /*Si el usuario perdió todo su dinero, unicamente hace una pausa y continua a mostrar el resumen de su resultado
                      en la sección de RESULTADOFINAL*/
                    if (gananciatotal == 0)
                    {
                        Console.ReadKey();
                    }

                    /*En caso el usuario obtenga alguna ganancia por minima que sea, se le consulta si desea apostar nuevamente
                      sus ganacia.*/
                    CONTINUAR3:
                    if (gananciatotal > 0)
                    {
                        Console.Write("\nDesea apostar nuevamente sus ganancias (1.Si / 2.No)\nIngrese número de opción : ");
                        switch (Console.ReadLine())
                        {
                            /*En el caso el usuario desee apostar nuevamente sus ganancias, la gananicatotal se resetea a 0 y el
                              nuevo monto de apuesta se iguala a la gananaciaacumulada del usuario y se vuelve a inicar el juego*/
                            case "1":
                                gananciatotal = 0;
                                monto = ganaciaacumulada;
                                goto JUEGO;
                            /*En el caso no desee continuar se le traslada a la sección de RESULTADOFINAL*/
                            case "2":
                                goto RESULTADOFINAL;
                            default:
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("\nUsted ha ingresado un dato no válido");
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ReadKey();
                                goto CONTINUAR3;
                        }
                    }

                    /*En la sección de RESULTADOFINAL muestra el resumen final de las ganancias o pérdidas del jugador*/
                RESULTADOFINAL:
                    /*Se declararon las variables para determinar el valor de los impuestos y de cuanto sería la gananciafinal de 
                      dinero que le queda al usuario despues de los impuestos*/
                    double impuestos = 0, gananciafinal=0;

                    /*Ya que la unica manera de determinar los impuestos es si hubo una ganancia, se hace uso de un if para validar
                      dicha información*/
                    if (gananciatotal > 0)
                    {
                        /*Los impuestos se calcularon determinando cuales habian sido unicamente las ganancias y a ese valor determinarle
                          su 40% los cuales debian de ser restados al total*/
                        impuestos = Math.Round(((gananciatotal - montoinicial) * 0.40), 2);
                        gananciafinal = ganaciaacumulada - impuestos;
                    }

                    /*En esta parte se limpia la consola, y se muestra un resumen de los datos personales del usuario y la ganancia obtenida
                      despues de impuestos, luego hace una pausa y se le redirige al Menú Principal*/
                    Console.Clear();
                    Console.WriteLine("\tRESULTADO FINAL");
                    Console.WriteLine("\nNombre: "+Nombre+"\nIdentificación: "+ID+"\nNacionalidad: "+Nacionalidad+"\nEdad: "+MAYOR21+" años\nGanancia Final (-40% impuestos sobre ganancias): Q"+gananciafinal);
                    Console.ReadKey();
                    goto MENU;

                    /*En el caso de que el usuario desee salir, se utiliza el comando de Enviroment.Exit(0) el cua cierra la aplicación
                      de consola*/
                case "2":
                    Environment.Exit(0);
                    break;

                    /*Continuando siempre con la validación de errores, en el caso el usuario ingrese una opción no válida se le notifica en color
                      rojo y lo regresa nuevamente al Menú Principal */
                default:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Opción no válida");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ReadKey();
                    goto MENU;
            }
            Console.ReadKey();
            
        }
    }
}
