﻿

using System.Collections.Generic;
using System.Threading.Tasks;
using CCK_App.Models;
using SQLite;

namespace CCK_App
{
    public class DatabaseLocal
    {
        SQLiteAsyncConnection db;

        public DatabaseLocal(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<Clientes>().Wait();
        }

        public Task SaveClientesAsync(Clientes clients)
        {
            if (clients.id==0)
            {
                return db.InsertAsync(clients);
            }
            else
            {
                return null;
            }
        }

        public Task<List<Clientes>> GetClienteByDni(string dni)
        {
            return db.Table<Clientes>().Where(c => c.dni == dni).ToListAsync();
        }

        public Task<List<Clientes>> GetAllClientes()
        {
            return db.Table<Clientes>().ToListAsync();
        }
    }
}