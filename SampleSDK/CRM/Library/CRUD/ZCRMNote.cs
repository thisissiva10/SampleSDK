using System.Collections.Generic;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.Setup.Users;

namespace SampleSDK.CRM.Library.CRUD
{
    public class ZCRMNote : ZCRMEntity
    {

        private long id;
        private string title;
        private string content;
        private ZCRMRecord parentRecord;
        private ZCRMUser notesOwner;
        private ZCRMUser createdBy;
        private string createdTime;
        private ZCRMUser modifiedBy;
        private string modifiedTime;
        private List<ZCRMAttachment> attachments = new List<ZCRMAttachment>();


        private ZCRMNote(ZCRMRecord parentRecord, long noteId)
        {
            ParentRecord = parentRecord;
            Id = noteId;

        }

        public ZCRMNote(ZCRMRecord parentRecord)
        {
            
        }

        public static ZCRMNote GetInstance(ZCRMRecord parentRecord, long noteId)
        {
            return new ZCRMNote(parentRecord, noteId);
        }


        public ZCRMRecord ParentRecord { get => parentRecord; private set => parentRecord = value; }

        public long Id { get => id; set => id = value; }

        public string Title { get => title; set => title = value; }

        public string Content { get => content; set => content = value; }

        public ZCRMUser NotesOwner { get => notesOwner; set => notesOwner = value; }

        public ZCRMUser CreatedBy { get => createdBy; set => createdBy = value; }

        public string CreatedTime { get => createdTime; set => createdTime = value; }

        public ZCRMUser ModifiedBy { get => modifiedBy; set => modifiedBy = value; }

        public string ModifiedTime { get => modifiedTime; set => modifiedTime = value; }

        public List<ZCRMAttachment> Attachments { get => attachments; private set => attachments = value; }



        public void AddAttachment(ZCRMAttachment attachment)
        {
            Attachments.Add(attachment);
        }
    }
}
