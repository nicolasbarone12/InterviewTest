using Repositories;
using Repositories.Interfaces;
using Repositories.ModelsDB;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PatientService:IService
    {

        private IRepository _patientRepository;

        public PatientService()
        {
            this._patientRepository = new PatientRepository();
        }
        public IEnumerable<Patient> GetAllPatients()
        {
            ICollection<Patient> patients = new List<Patient>();

            var patientsEntities = this._patientRepository.GetAll();
            foreach(PatientEntity patient in patientsEntities)
            {
                patients.Add(new Patient()
                {
                    DateOfBirth = patient.DateOfBirth,
                    Name = patient.Name,    
                    LastName = patient.LastName,
                    PatientId = patient.PatientId,
                    Sickness = patient.Sickness,
                });
            }

            return patients;
        }

        public bool InsertNewPatient(Patient patient)
        {
            bool patientInserted = false;
            PatientEntity patientEntity = new PatientEntity()
            {
                DateOfBirth = patient.DateOfBirth,
                Name = patient.Name,
                LastName = patient.LastName,
                Sickness = patient.Sickness,
            };

            patientInserted = (this._patientRepository.Insert(patientEntity) == 1);

            return patientInserted;
        }

        public bool UpdatePAtient(Patient patient)
        {
            bool patientUpdated = false;
            PatientEntity patientEntity = new PatientEntity()
            {
                DateOfBirth = patient.DateOfBirth,
                Name = patient.Name,
                LastName = patient.LastName,
                Sickness = patient.Sickness,
                PatientId= patient.PatientId,
            };

            patientUpdated = (this._patientRepository.Update(patientEntity));

            return patientUpdated;
        }

        public bool DeletePatient(Patient patient)
        {
            PatientEntity patientEntity = new PatientEntity()
            {
                PatientId = patient.PatientId,
            };

            var patientDeleted = this._patientRepository.Delete(patientEntity);

            return patientDeleted;
        }

        public Patient GetById(int id)
        {
            
            PatientEntity patient = (PatientEntity)this._patientRepository.GetById(id);
            return new Patient()
            {
                DateOfBirth = patient.DateOfBirth,
                LastName = patient.LastName,
                PatientId = patient.PatientId,
                Name = patient.Name,
                Sickness = patient.Sickness,
            };
        }

        public IEnumerable<Patient> GetByFilters(string filters)
        {
            ICollection<Patient> patients = new List<Patient>();
            var entities =  this._patientRepository.GetByFilters(filters);

            foreach (PatientEntity entity in entities)
            {
                patients.Add(new Patient()
                {
                    DateOfBirth = entity.DateOfBirth,
                    LastName = entity.LastName,
                    PatientId = entity.PatientId,
                    Name = entity.Name,
                    Sickness = entity.Sickness,
                });
            }

            return patients;
        }
    }
}
