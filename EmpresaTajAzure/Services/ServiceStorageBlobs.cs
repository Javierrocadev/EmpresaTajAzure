using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using EmpresaTajAzure.Models;

namespace EmpresaTajAzure.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;

        public ServiceStorageBlobs(BlobServiceClient client)
        {
            this.client = client;
        }

        //METODO PARA MOSTRAR LOS CONTAINERS
        public async Task<List<string>>
            GetContainersAsync()
        {
            List<string> containers = new List<string>();
            await foreach (BlobContainerItem item in
                this.client.GetBlobContainersAsync())
            {
                containers.Add(item.Name);
            }
            return containers;
        }

        //METODO PARA CREAR UN CONTENEDOR
        public async Task CreateContainerAsync(string containerName)
        {
            await this.client.CreateBlobContainerAsync
                (containerName, PublicAccessType.Blob);
        }

        //METODO PARA ELIMINAR UN CONTENEDOR
        public async Task DeleteContainerAsync(string containerName)
        {
            await this.client.DeleteBlobContainerAsync(containerName);
        }





        //METODO PARA MOSTRAR TODOS LOS BLOBS DE UN CONTAINER
        public async Task<List<BlobModel>>
            GetBlobsAsync(string containerName)
        {
            //RECUPERAMOS EL CLIENT DEL CONTAINER
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            List<BlobModel> models = new List<BlobModel>();
            await foreach (BlobItem item in
                containerClient.GetBlobsAsync())
            {
                //name, containerName, Url
                //DEBEMOS CREAR UN BLOBCLIENT SI NECESITAMOS 
                //TENER MAS INFORMACION DEL BLOB
                BlobClient blobClient =
                    containerClient.GetBlobClient(item.Name);
                BlobModel blob = new BlobModel();
                blob.Nombre = item.Name;
                blob.Contenedor = containerName;
                blob.Url = blobClient.Uri.AbsoluteUri;
                models.Add(blob);
            }
            return models;
        }



        //METODO PARA ELIMINAR UN BLOB DE UN CONTAINER
        public async Task DeleteBlobAsync(string containerName
            , string blobName)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            await containerClient.DeleteBlobAsync(blobName);
        }



        //METODO PARA SUBIR UN BLOB A UN CONTAINER
        public async Task UploadBlobAsync(string containerName
            , string blobName, Stream stream)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            await containerClient.UploadBlobAsync
                (blobName, stream);
        }
    }

}
