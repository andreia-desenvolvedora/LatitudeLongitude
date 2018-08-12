using LatitudeLongitude.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Informar nome de amigos e suas coordenadas (latitude e longitude)
/// para então retornar os 3 amigos mais próximos uns dos outros
/// </summary>
namespace LatitudeLongitude
{
    /// <summary>
    /// Informar nome de amigos e suas coordenadas (latitude e longitude)
    /// para então retornar os 3 amigos mais próximos uns dos outros
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            #region Mock Test
            List<FriendsDTO> lstMockTest = new List<FriendsDTO>
            {
                new FriendsDTO() { FriendName = "Arco do Triunfo", Latitude = 48.8737917F, Longitude = 2.2928388F }, // Europa
                new FriendsDTO() { FriendName = "Magic Kingdom", Latitude = 28.417663F, Longitude = -81.581212F }, // América do Norte
                new FriendsDTO() { FriendName = "Hopi Hari", Latitude = -23.0973996F, Longitude = -47.0113408F }, // América do Sul
                new FriendsDTO() { FriendName = "Torre Eiffel", Latitude = 48.8583698F, Longitude = 2.2922946F }, // Europa
                new FriendsDTO() { FriendName = "Universal Studios", Latitude = 28.47435F, Longitude = -81.4700037F }, // América do Norte
                new FriendsDTO() { FriendName = "Beto Carrero", Latitude = -26.8016998F, Longitude = -48.6197606F }, // América do Sul
                new FriendsDTO() { FriendName = "Museu do Louvre", Latitude = 48.8606111F, Longitude = 2.3354553F }, // Europa
                new FriendsDTO() { FriendName = "Disney Paris", Latitude = 48.8722344F, Longitude = 2.7736192F }, // Europa
                new FriendsDTO() { FriendName = "Playcenter", Latitude = -23.542538F, Longitude = -46.6558977F }, // América do Sul
                new FriendsDTO() { FriendName = "Hollywood Studios", Latitude = 28.3575294F, Longitude = -81.5604601F }, // América do Norte
                new FriendsDTO() { FriendName = "Parque da Mônica", Latitude = -23.677725F, Longitude = -46.7015527F }, // América do Sul
                new FriendsDTO() { FriendName = "Busch Gardens", Latitude = 28.0332331F, Longitude = -82.4178031F } // América do Norte
            };

            Console.WriteLine("*** INÍCIO DOS DADOS MOCKADOS ***");
            FindFriends(lstMockTest); // Localizar locais
            Console.WriteLine();
            Console.WriteLine("*** FIM DOS DADOS MOCKADOS ***");
            Console.ReadKey();
            Console.Clear();

            #endregion Mock Test

            #region Amigos

            List<FriendsDTO> lstFriends = AddFriends(); // Inserir amigos e coordenadas
            if (lstFriends == null)
            {
                Console.WriteLine();
                Console.WriteLine($"Você não inseriu nenhum amigo.");
            }
            else
            {
                Console.WriteLine($"Você inseriu {lstFriends.Count} amigos.");
                Console.WriteLine();
                FindFriends(lstFriends); // Localizar amigos
            }
            Console.ReadKey();

            #endregion Amigos
        }

        /// <summary>
        /// Adicionar o nome, latitude e longitude dos amigos
        /// </summary>
        /// <returns></returns>
        private static List<FriendsDTO> AddFriends()
        {
            try
            {
                #region Definição das variáveis
                List<FriendsDTO> lstFriends = new List<FriendsDTO>();
                string friendName = string.Empty;
                float latitude, longitude = 0F;
                int totalFriends = 0;
                #endregion Definição das variáveis

                #region Quantidade de amigos
                Console.Write("Quantos amigos você quer informar? ");
                Int32.TryParse(Console.ReadLine(), out totalFriends);
                if (totalFriends < 3)
                {
                    throw new Exception("Você precisa informar pelo menos 3 amigos");
                }
                #endregion Quantidade de amigos

                #region Informações dos amigos
                Console.WriteLine();
                Console.WriteLine("Informe o nome de seu amigo, sua latitude e longitude.");

                for (int cont = 0; cont < totalFriends; cont++)
                {
                    try
                    {
                        Console.WriteLine();
                        Console.Write("Nome de seu amigo: ");
                        friendName = Console.ReadLine();
                        if (string.IsNullOrEmpty(friendName))
                        {
                            throw new Exception("O nome do seu amigo deve ser informado.");
                        }

                        Console.Write("Latitude de seu amigo: ");
                        float.TryParse(Console.ReadLine().Replace(".", ","), out latitude);
                        if (latitude == 0)
                        {
                            throw new Exception("A latitude do seu amigo deve ser informada.");
                        }

                        Console.Write("Longitude de seu amigo: ");
                        float.TryParse(Console.ReadLine().Replace(".", ","), out longitude);
                        if (longitude == 0)
                        {
                            throw new Exception("A longitude do seu amigo deve ser informada.");
                        }

                        lstFriends.Add(new FriendsDTO()
                        {
                            FriendName = friendName,
                            Latitude = latitude,
                            Longitude = longitude
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"A informação inserida está incorreta. Este registro será ignorado - Erro [{ex.Message}].");
                        cont--;
                    }
                }

                Console.WriteLine();
                #endregion Informações dos amigos

                return lstFriends;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"A informação inserida está incorreta. Execute novamente - Erro [{ex.Message}].");
            }

            return null;
        }

        /// <summary>
        /// Mostrar os 3 amigos mais próximos
        /// Baseado no cálculo raizQuadrada((x2 - x1)2 + (y2 - y1)2)
        /// </summary>
        /// <param name="lstFriends"></param>
        private static void FindFriends(List<FriendsDTO> lstFriends)
        {
            if (lstFriends == null)
            {
                Console.WriteLine("Não há amigos para exibir.");
            }
            else
            {
                #region Definição das variáveis
                List<FriendsDTO> lstNearbyFriends = lstFriends;
                List<DistanceDTO> lstDistance = null;
                string nearbyFriends = string.Empty;
                #endregion Definição das variáveis

                foreach (FriendsDTO friend in lstFriends)
                {
                    lstDistance = new List<DistanceDTO>();
                    foreach (FriendsDTO nearbyFriend in lstNearbyFriends.Where(aa => aa.FriendName != friend.FriendName))
                    {
                        lstDistance.Add(new DistanceDTO()
                        {
                            FriendName = nearbyFriend.FriendName,
                            Distance = Math.Sqrt((Math.Pow(nearbyFriend.Latitude - friend.Latitude, 2)) + (Math.Pow(nearbyFriend.Longitude - friend.Longitude, 2)))
                        });
                    }

                    nearbyFriends = String.Join(", ", lstDistance.OrderBy(aa => aa.Distance).Take(3).Select(aa => aa.FriendName).ToArray());

                    Console.WriteLine();
                    Console.WriteLine($"Amigos nas proximidades de {friend.FriendName}:");
                    Console.WriteLine($"   {nearbyFriends}");
                }
            }
        }
    }
}
