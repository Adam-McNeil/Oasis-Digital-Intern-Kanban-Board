using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;

[FirestoreData]
public struct Ticket {

    [FirestoreProperty]
    public string Title { get; set; }

    [FirestoreProperty]
    public string Description { get; set; }

    [FirestoreProperty]
    public string Status { get; set; }

    [FirestoreProperty]
    public string TicketReporter { get; set; }

    [FirestoreProperty]
    public string AssignedUser { get; set; }

    [FirestoreProperty]
    public long DateCreated { get; set; }

    [FirestoreProperty]
    public long LastUpdated { get; set; }

}

public class FirebaseUploader : MonoBehaviour
{
    private FirebaseApp app;
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                app = Firebase.FirebaseApp.DefaultInstance;
            }
            else {
                UnityEngine.Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createTicket() {
        var date = DateTimeOffset.Now.ToUnixTimeSeconds();
        var ticket = new Ticket {
            Title = "Test",
            Description = "Whatever",
            Status = "On Hold",
            TicketReporter = "Me",
            AssignedUser = null,
            DateCreated = date,
            LastUpdated = date
        };
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Collection("tickets").Document().SetAsync(ticket);
    }
}
