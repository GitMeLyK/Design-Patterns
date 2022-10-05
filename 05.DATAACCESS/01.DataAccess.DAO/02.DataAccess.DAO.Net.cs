using System;

namespace DotNetDesignPatternDemos.DataAccess.DAO.Net
{

    /*
     * Il modello DAO in ambiente microsoft è stato per termine e nome usato e convenzionato
     * come lo stesso nome del pattern a cui si attiene.
     * 
     * Qesta libreria utilizzata in diversi ambienti microsoft non solo .net ma anche vba
     * visual basic etc. è stata sempre la porta di accesso per la connessione a db proprietari
     * della stessa microsoft in primi ACCESS.
     * 
     * Nell'eveoluzione però di questo modello di progettazione ed implmentazione con questa dll
     * però sono avvenute delle circostanze in cui il progetto DAO libreria non aveva più motivo
     * di essere chiamato tale e di essere stato rifatto e modellatto secondo altri principi come
     * quello verso la connessione a database server come SQL server etc.
     * 
     * Lo stesso Visual C++ e le procedure guidate non supportano più DAO (anche se 
     * le classi DAO sono incluse ed è comunque possibile usarle). 
     * Microsoft consiglia di usare ODBC per i nuovi progetti MFC. 
     * È consigliabile usare DAO solo per gestire le applicazioni esistenti.
     * 
     * Facciamo un pò di chiarezza per .Net DAO che è anche acronimo dell'oggetto offerto
     * dal framework per l'accesso ai dati che è chiamato DAO come lo stesso pattern, però
     * la sua evoluzione di implementazione è arrivata fino alla version DAO 3.6 sempre 
     * presente nel framework .Net ed è come versione finale considerato ormai obsoleto, 
     * queste classi funzionano con le altre classi del framework applicazione per fornire 
     * un facile accesso ai database DAO (Data Access Object), che usano lo stesso motore 
     * di database di Microsoft Visual Basic e Microsoft Access. 
     * Le classi DAO possono anche accedere a un'ampia gamma di database per cui sono 
     * disponibili Open Database Connectivity (ODBC).
     * 
     * DAO, quindi l'oggetto del framework di Microsoft risponde bene alla necessità di 
     * connettersi ad un database Access della stessa Microsoft mentre per l'accesso al
     * Server SQL di Microsoft è necessario un altro tipo di componente chiamato ADODB.
     * 
     * La domanda implica che DAO o ADODB si escludono a vicenda, ma non lo sono. 
     * Potresti facilmente creare un'app Access usando solo DAO che funzionerebbe bene, 
     * ma sarebbe difficile creare la stessa app usando solo ADODB, ci vorrebbe molto più 
     * codice e troppi compromessi. 
     * 
     * Resta il fatto che DAO è e continuerà ad essere la tecnologia predefinita da 
     * utilizzare in tutto Access.
     * 
     * Quindi DAO è solo la strada da percorrere? No, utilizzare ADODB per accedere 
     * direttamente a SQL Server se si desidera ottimizzare la relazione tra Access e SQL Server, 
     * è NECESSARIO utilizzare ADODB per ottenere la massima efficienza/ROI nel codice. 
     * 
     * Il trucco è sapere quando usare DAO e quando usare ADODB.
     * 
     * Sorge una domanda ADODB è più veloce di DAO?
     * 
     *  Questa è la domanda sbagliata, piuttosto, perché SQL Server è più veloce di Access 
     *  dovrebbe essere la domanda. Se si chiede ad Access di aggiornare 100.000 record 
     *  utilizzando una query di aggiornamento di Access nativa rispetto all'utilizzo di 
     *  ADODB, prepararsi a prendere una o due tazze di caffè durante l'attesa. 
     *  
     *  In generale, se il processo verrà eseguito più velocemente sul server (e quasi sempre 
     *  lo fanno), utilizzare ADODB, altrimenti utilizzare DAO.
     *  
     *  Questo è stato un excursus per quanto riguarda le implementazioni intorno a questo
     *  modello architetturale in casa Microsoft, nel prossimo capitolo parliamo dell'evoluzione
     *  da parte del framework di implementazioni ed evolutive fatte per .Net e antecedenti.
     *  
     */

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
