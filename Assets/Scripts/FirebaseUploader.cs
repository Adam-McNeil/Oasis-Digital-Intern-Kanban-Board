using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [FirestoreProperty]
    public double X { get; set; }

    [FirestoreProperty]
    public double Y { get; set; }

    [FirestoreProperty]
    public double Z { get; set; }

}

public class FirebaseUploader : MonoBehaviour
{
    private FirebaseApp app;

    public TMP_InputField titleInput;
    public TMP_InputField descriptionInput;
    public TMP_Dropdown statusInput;
    public TMP_Dropdown assignedInput;

    void Start() {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                app = Firebase.FirebaseApp.DefaultInstance;
            }
            else {
                Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    void Update() {
        
    }

    public void createTicket() {
        createTicketReturnId();
    }

    public string createTicketReturnId() {
        var ticket = createTicketTemplate();
        ticket.Title = titleInput.text;
        ticket.Description = descriptionInput.text;
        ticket.Status = statusInput.options[statusInput.value].text;
        ticket.AssignedUser = assignedInput.options[assignedInput.value].text;
        var firestore = FirebaseFirestore.DefaultInstance;
        var document = firestore.Collection("tickets").Document();
        document.SetAsync(ticket);
        return document.Id;
    }

    // public void updateTicket(string id) {
    //     var firestore = FirebaseFirestore.DefaultInstance;
    //     var document = firestore.Collection("tickets").Document(id);
    //     document.SetAsync(createTicketTemplate());
    // }

    private long getCurrentDate() {
        return DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    private Ticket createTicketTemplate(string title = "Test", string description = "Whatever", string status = "On Hold", string reporter = "Me", string assignedUser = null) {
        var date = getCurrentDate();
        return new Ticket {
            Title = title,
            Description = description,
            Status = status,
            TicketReporter = reporter,
            AssignedUser = assignedUser,
            DateCreated = date,
            LastUpdated = date,
            X = 0,
            Y = 0,
            Z = 0
        };
    }
}
