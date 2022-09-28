using System;

namespace Behavior.Facade
{

    /*
     *  https://github.com/ActiveMesa/MdxConsole
     *  Questo esempio fà vedere un cìprogetto console e come opera nel contesto
     *  del facade per rendere attraverso un interfaccia pubbliche quelle API da
     *  usare nel contesto del consumer.
     *  Fà si che la console con tutti i suoi meccanismi interni venga fondamentalmente
     *  usata con l'ausilio dell'interfaccia attraverso API semplici da gestire e
     *  monitorare.
     *  L'interfaccia in questione IExposedBuffer espone tre semplici api con due metodi
     *  per scrivere sulla console Write(..) e WriteLine(..) questa è per chi opera
     *  nel contesto di programmazione delle api l'interfaccia di riferimento.
     *  La console in sè è resa disponibile attraverso l'uso delle directx che accellerrano
     *  il rendring in output e questa rende una vissta della console migliorata.
     *  Il punto di doamanda è perchè è lenta una console nel rendering, è presto detto,
     *  ogni qualvolta la console in questione usando i caratteri vettoriali di windows ha
     *  necessariamente bisogno di prelevare il carattere vettoriale dal font e printarlo 
     *  a schermo rasterizzandolo a bitmap o carattere raw, ecco perchè la bontà di questo
     *  mini programma è utile farglielo fare in un accelleratore hardware dovendo rendere
     *  disponibile per lìutente la possibilità di usare i caratteri di windows su console.
     *  Qui internamente entra in gioco una delle funzioni principali TextureManager che
     *  opera nel contesto di output della console e rasterizza prima un elenco di font
     *  e fatto ciò li rende disponibile per le successive operazioni di write su console.
     *  Quindi vediamo che il gestore delle texture è un sottositema per intendere come
     *  il pattern Facade opera, e quindi essendo un sottositema fa parte della console generale
     *  e non volendo comlicare le API da usare questo viene nascosto nel dettaglio di 
     *  implementazione non facendo intarigire troppo l'utilizzatore delle API, ancora abbiamo
     *  un implementazione di più buffer e viste multiple, dove per buffer intendiamo porzioni
     *  di memoria schermo dove ci sono cose disegnate una sola volta e printate successivamente
     *  riprendendo il contesto già renderizzato, e per il Buffer viene anche esposta una
     *  interfaccia di facciata per il consumer che ne espone il get e il set. L'interfaccia
     *  per il consumer quindi si occupa solo di scrivere sulla console e non vede lo strato di
     *  facciata dove viene ripresa la viewport corretta o altro della reale implementazione 
     *  della console. Un esempio di come le api in questo contesto finale di utilizzo vengono
     *  usate possono essere viste nel main di questo file.
     *  Per riassumere la console in questo caso agisce come una sorta di gateway e nella facciata
     *  vediamo un output quindi vediamo cosa effettivamente stiamo facendo.
     *  Nell'esempio se dovessimo operare in un sacco di finestre e avere un sacco di buffer
     *  vediamo ancora come la facciata interagisce con la funzione in questo caso il Witeline
     *  scriverà sulla finestra attiva che è impostata in uno stato interno rispetto alle altre.
     */


    class Program
    {
        static void Main(string[] args)
        {
            /*
            TableBuffer myBuffer = new TableBuffer(new[] {
                    new TableBuffer.TableColumnsSpec("Current", 10),
                    new TableBuffer.TableColumnsSpec("Change", 10)
                }
            )
            Random random = new Random();

            for (int i=0; i < myBuffer.Size.Height -1; i++)
            {
                myBuffer.Buffer[i][0] = random.Next(1, 1000).ToString();
                myBuffer.Buffer[i][1] = random.NextDouble().ToString();
            }

            Viewport view = new Viewport(myBuffer);
            view.ScreenLocation = new point(20, 10);

            using (var c= Console.Create(ccp))
            {
                c.Viewports.add(view);

                c.Text = "Demo view";
                c.FormBorderStyle = FormBorderStyle.FixedSingle;

                var yellow = c.TexttureManager.AddPreset(ConsoleColor.Black, Colors.Yellow, ConsoleFonts.consolenew);

                c.KeyUp += (sender, key) =>
                {
                    foreach (var vp in c.viewPorts)
                    {
                        vp.OnKeyEvent(sender, key);
                    }
                };
                c.show();
            }
            */
        }
    }
}
