using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EmailMicrogame : MonoBehaviour
{
    private string[] goodSenders = { "John Smith", "David Lee", "Emily Rodriguez" };
    private string[] goodAdress = { "john.smith@company.com", "david.lee@company.com", "emily.rodriguez@company.com" };
    private string[] goodEmail = { "Dear Player,\r\n\r\nI hope this email finds you well. I would like to schedule a meeting to discuss the progress of Project X and address any challenges we may be facing. Could we arrange a convenient time for you to meet sometime this week?\r\n\r\nPlease let me know your availability, and I will coordinate the meeting accordingly.\r\n\r\nBest regards,\r\nJohn Smith",
        "Dear Player,\r\n\r\nI hope this email finds you well. Attached is the proposal document for Project 2 as discussed. Please review the document at your earliest convenience, and let me know if you have any questions or require further clarification.\r\n\r\nWe are excited about the opportunity to work with you and are looking forward to your feedback.\r\n\r\nBest regards,\r\nDavid Lee\r\n",
        "Dear Player,\r\n\r\nI hope you're doing well. I am writing to inquire about the service offered by your company. Could you please provide me with more information regarding its features, pricing, and any ongoing promotions?\r\n\r\nYour prompt response would be greatly appreciated.\r\n\r\nThank you and best regards,\r\nEmily Rodriguez\r\n" };
    private string[] goodSubject = { "Meeting Request for Project X Discussion", "Submission of Proposal for Project 2", "Inquiry Regarding the Service" };

    private string[] badAdress = { "john.smith@opendomain.com", "david.lee@opendomain.com", "emily.rodriguez@opendomain.com" };
    private string[] badEmail = { "Dear Player,\r\n\r\nI hope this email finds you well. I would like to schedule a meeting to discuss the progress of Project X and address any challenges we may be facing. Could you provide me with your full schedule for this week so I can organise a time that works for both of us?\r\n\r\nPlease let me know soon, and I will coordinate the meeting accordingly.\r\n\r\nBest regards,\r\nJohn Smith",
        "Dear Player,\r\n\r\nI hope this email finds you well. Attached is the proposal document for Project 2 as discussed. Please open the program and follow all steps to download the proposal document.\r\n\r\nWe are excited about the opportunity to work with you and are looking forward to your feedback.\r\n\r\nBest regards,\r\nDavid Lee",
        "Dear Player,\r\n\r\nI hope you're doing well. I am writing to inquire about the service offered by your company. Could you please provide me with more information regarding its features, pricing, and any ongoing promotions? Additionally, if you could provide a username and password to access the service for us to test before we make a decision we would be incredibly grateful.\r\n\r\nYour prompt response would be greatly appreciated.\r\n\r\nThank you and best regards,\r\nEmily Rodriguez" };
    private string[] badSender = { "Meting Request for Progect X Diskusion", "Sabmision of Propsal for Projekt 2", "Incuiry Regrdin the Serfice" };

    public TMP_Text[] mainEmail = new TMP_Text[4];
    public TMP_Text[] fakeEmail = new TMP_Text[4];

    private int choice;
    private int issues;
    private int newSpot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChooseEmail()
    {
        choice = UnityEngine.Random.Range(0, 2);

        mainEmail[0].text = goodSubject[choice];
        mainEmail[1].text = goodSenders[choice];
        mainEmail[2].text = goodAdress[choice];
        mainEmail[3].text = goodEmail[choice];
    }

    private void makeIssues()
    {
        issues = Random.Range(0, 2);
        fakeEmail[0].text = goodSubject[choice];

        if (issues == 2)
        {
            fakeEmail[1].text = badSender[choice];
            fakeEmail[2].text = badAdress[choice];
            fakeEmail[3].text = badEmail[choice];
        }
        else if (issues == 1) 
        {
            int spot = Random.Range(0, 2);
            if (spot == 2)
            {
                fakeEmail[3].text = badEmail[choice];
            }
            else if (spot == 1)
            {
                fakeEmail[2].text = badAdress[choice];
            }
            else
            {
                fakeEmail[1].text = badSender[choice];
            }

            while (newSpot == spot)
            {
                newSpot = Random.Range(0, 2);
            }
        }
    }
}