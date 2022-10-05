using System;

namespace DotNetDesignPatternDemos.Architectural.FrontController.Example
{
	/*
     *  Il modello di progettazione del front controller indica che tutte le richieste 
     *  relative a una risorsa in un'applicazione verranno gestite da un singolo gestore 
     *  e quindi inviate al gestore appropriato per quel tipo di richiesta. 
     *  
     *  Il front controller può utilizzare altri aiutanti per ottenere il meccanismo di 
     *  dispacciamento.
     *  
     *      // Figura 1 : uml-front-controller-design-pattern.png
     *      
     *  Controller: Il controller è il punto di contatto iniziale per la gestione di tutte 
     *              le richieste nel sistema. Il controller può delegare a un helper di 
     *              completare l'autenticazione e l'autorizzazione di un utente o di 
     *              avviare il recupero dei contatti.
     *              
     *  Vista:      Una vista rappresenta e visualizza le informazioni al client. 
     *              La vista recupera le informazioni da un modello. Gli helper 
     *              supportano le visualizzazioni incapsulando e adattando il modello di 
     *              dati sottostante per l'utilizzo nella visualizzazione.
     *              
     *  Dispatcher: Un dispatcher è responsabile della gestione della visualizzazione e 
     *              della navigazione, gestendo la scelta della vista successiva da 
     *              presentare all'utente e fornendo il meccanismo per il controllo 
     *              vettoriale di questa risorsa.
     *              
     *  Helper:     Un helper è responsabile di aiutare una vista o un controller a 
     *              completare la sua elaborazione. Pertanto, gli aiutanti hanno numerose 
     *              responsabilità, tra cui la raccolta dei dati richiesti dalla vista e 
     *              l'archiviazione di questo modello intermedio, nel qual caso l'helper 
     *              viene talvolta indicato come un fagiolo di valore.
     *              
     * Vantaggi:
     *				
     *		Controllo centralizzato : Il front controller gestisce tutte le richieste 
     *								 all'applicazione Web. Questa implementazione del controllo 
     *								 centralizzato che evita l'utilizzo di più controller è 
     *								 auspicabile per l'applicazione di criteri a livello di 
     *								 applicazione, ad esempio il monitoraggio e la sicurezza 
     *								 degli utenti.
     *		
     *		Sicurezza della filettatura : Un nuovo oggetto comando si verifica quando si riceve 
     *								 una nuova richiesta e gli oggetti comando non sono pensati 
     *								 per essere thread-safe. Pertanto, sarà sicuro nelle classi 
     *								 di comando. Sebbene la sicurezza non sia garantita quando 
     *								 vengono raccolti problemi di threading, i codici che agiscono 
     *								 con il comando sono comunque thread safe.
     * Difetti:
     * 
     *		Non è possibile ridimensionare un'applicazione utilizzando un front controller.
     *		
     *		Le prestazioni sono migliori se gestisci una singola richiesta in modo univoco.
     *		
     * In questo esempio viene solo abbozzato un tipico uso di un front controller, non è
     * un codice di esempio da mettere in produzione, ma rende l'idea del dispatcher come
     * agisce e del controller.
     * 
    */

	class TeacherView
	{
		public void display()
		{
			System.Console.Write("Teacher View");
		}
	}

	class StudentView
	{
		public void display()
		{
			System.Console.Write("Student View");
		}
	}

	class Dispatching
	{
		private StudentView studentView;
		private TeacherView teacherView;

		public Dispatching()
		{
			studentView = new StudentView();
			teacherView = new TeacherView();
		}

		public void dispatch(String request)
		{
			if (request.Equals("Student"))
			{
				studentView.display();
			}
			else
			{
				teacherView.display();
			}
		}
	}

	class FrontController
	{
		private Dispatching Dispatching;

		public FrontController()
		{
			Dispatching = new Dispatching();
		}

		private bool isAuthenticUser()
		{
			System.Console.Write("Authentication successful.");
			return true;
		}

		private void trackRequest(String request)
		{
			System.Console.Write("Requested View: " + request);
		}

		public void dispatchRequest(String request)
		{
			trackRequest(request);

			if (isAuthenticUser())
			{
				Dispatching.dispatch(request);
			}
		}
	}

	class FrontControllerPattern
	{
		public static void main(String[] args)
		{
			/*
			 *	
			 *	Requested View: Teacher
				Authentication successful.
				Teacher View
				Requested View: Student
				Authentication successful.
				Student View
			*/

			FrontController frontController = new FrontController();
			frontController.dispatchRequest("Teacher");
			frontController.dispatchRequest("Student");
		}
	}


}
