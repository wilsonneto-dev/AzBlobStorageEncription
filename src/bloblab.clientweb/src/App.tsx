import axios from "axios";
import { useEffect, useState } from "react";
import { FaDownload, FaCircle } from "react-icons/fa";

interface FileDTO {
  id: number;
  name: string;
  extension: string;
  security: string;
}

const Loading = () => (
  <div className="container mx-auto w-10">
    <span className="flex h-3 w-3 inline">
      <span className="animate-ping">
        <FaCircle />
      </span>
    </span>
  </div>
);

const App = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [filesList, setFilesList] = useState<Array<FileDTO>>([]);
  const [encript, setEncript] = useState(false);
  const [selectedFile, setSelectedFile] = useState<any>(null);
  const [message, setMessage] = useState("");

  useEffect(() => {
    fetchFiles();
  }, []);

  const fetchFiles = async () => {
    setIsLoading(true);
    setFilesList([]);
    const { data } = await axios.get<Array<FileDTO>>(
      "https://localhost:5001/files"
    );
    setFilesList(data);
    setIsLoading(false);
  };

  const uploadFile = async () => {
    const formData = new FormData();
    if (!selectedFile) {
      setMessage("selecione um arquivo");
      return;
    }
    setIsLoading(true);
    formData.append("file", selectedFile);
    if (encript) formData.append("encript", "true");
    const { data } = await axios.post("https://localhost:5001/files", formData);
    setMessage(`Upload bem sucedido, arquivo ${data.id}`);
    fetchFiles();
  };

  const getDownloadLink = async (id: number, security: string ) => {
    if(security.toLowerCase() === "encripted") {
      setMessage(`Arquivo encriptado, download feito pelo backend: \nhttps://localhost:5001/files/${id}`);
      return;
    }
    setMessage("Buscando informações...")
    setIsLoading(true);
    const { data } = await axios.get(`https://localhost:5001/files/${id}/link`);
    setIsLoading(false);
    setMessage(`Arquivo apenas privado, foi gerado um link de acesso temporário com uma SAS: \n${data.link}`);
  }

  return (
    <div className="App">
      <div className="container p-3 mx-auto mt-20 max-w-md bg-white rounded-lg shadow lg:w-1/3">
        <div className="flex flex-col">
          <input
            type="file"
            name="file"
            onChange={(e) => setSelectedFile(e.target?.files?.[0])}
          />
          <label>
            <input
              type="checkbox"
              name="encrypt"
              onChange={(e) => setEncript(e.target.checked)}
            />
            &nbsp; Encriptar
          </label>
          <button
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-1 px-4 rounded mt-1"
            onClick={uploadFile}
          >
            Enviar arquivo
          </button>
        </div>
      </div>

      {message !== "" && (
        <div
          className="overflow-hidden container mx-auto mt-5 max-w-md bg-blue-100 border-t border-b border-blue-500 text-blue-700 px-4 py-3"
          role="alert"
        >
          <p className="text-sm">{message}</p>
        </div>
      )}

      <div className="container mx-auto mt-5 max-w-md bg-white rounded-lg shadow lg:w-1/3 p-2">
        {isLoading && <Loading />}
        <ul className="divide-y divide-gray-100">
          {filesList.map((file) => (
            <li key={file.id} className="p-3 flex">
              <div className="flex-1">#{file.id} - {file.name}</div>
              <div
                className={`text-xs inline-flex ml-2 items-center leading-sm lowercase px-3 py-1 bg-${
                  file.security.toLowerCase() === "encripted" ? "blue" : "green"
                }-100 text-gray-500 rounded-full`}
              >
                {file.security}
              </div>
              <FaDownload
                className="self-end ml-5 cursor-pointer text-blue-500 hover:text-blue-700"
                onClick={() => getDownloadLink(file.id, file.security)}
              />
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default App;
