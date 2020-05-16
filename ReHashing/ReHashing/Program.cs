using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReHashing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            ReHashing ht = new ReHashing();
            ht.Add("tan", "187pm20569");
            ht.Add("cuong", "2223333");
            ht.printHash();
        }
    }

    public class Node
    {
        public object key;
        public object value;
        public Node next;

        public Node(object key, object value)
        {
            this.key = key;
            this.value = value;
            next = null;
        }
    }

    class ReHashing
    {
        Node[] buckets;
        int size;
        int numberBuckets;
        const double DEFAULT_LOAD_FACTOR = 0.75;

        #region constructor
        public ReHashing()
        {
            numberBuckets = 5;
            buckets = new Node[numberBuckets];
            for (int i = 0; i < numberBuckets; i++)
            {
                buckets[i] = null;
            }
            Console.WriteLine("HashMap created");
            Console.WriteLine("Number of pairs in the HashTable: " + size);
            Console.WriteLine("Size of HashTable: " + numberBuckets);
            Console.WriteLine("Default Load Factor : " + DEFAULT_LOAD_FACTOR + "\n");
        }
        #endregion

        #region Hàm biến đổi khóa key thành index trong mảng
        private int HashCode(object key)
        {
            var temp = key.ToString().ToCharArray();
            var result = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                result += temp[i] * (i + 1);
            }
            int hashcode = result.GetHashCode();
            return (hashcode % numberBuckets);
        }
        #endregion

        #region Thêm key và value vào hash table
        public void Add(object key, object value)
        {
            int bucketInd = HashCode(key);
            Node head = buckets[bucketInd];

            //kiểm tra node tại index có tồn tại chưa
            while (head != null)
            {
                // Nếu tồn tại thì cập nhật lại
                if (head.key.Equals(key))
                {
                    head.value = value;
                    return;
                }
                head = head.next;
            }

            // nếu null thì tạo node mới r add
            Node newElementNode = new Node(key, value);
            // đưa head trở lại vị trí index trong mảng ban đầu
            head = buckets[bucketInd];

            newElementNode.next = head;
            buckets[bucketInd]= newElementNode;
            Console.WriteLine("Thêm thành công node mới có key là {0} và value là {1}",key,value);
            size++;

            double loadFactor = (1.0 * size) / numberBuckets;
            Console.WriteLine("Hệ số tải = " + loadFactor);

            if (loadFactor > DEFAULT_LOAD_FACTOR)
            {
                Console.WriteLine(loadFactor + "Lớn hơn hệ số tải mặc định" + DEFAULT_LOAD_FACTOR);
                Console.WriteLine("Therefore Rehashing will be done.\n");

                // Rehash 
                rehash();

                Console.WriteLine("Độ dài của mảng: " + numberBuckets + "\n");
            }
            Console.WriteLine("Number of pairs in the Map: " + size);
            Console.WriteLine("Size of Map: " + numberBuckets + "\n");
        }
        #endregion

        #region Rehashing
        private void rehash()
        {

            Console.WriteLine("\n***Bắt Đầu ReHashing***\n");
 
            Node[] temp = buckets;

            buckets = new Node[2 * numberBuckets];

            for (int i = 0; i < 2 * numberBuckets; i++)
            {
                buckets[i] = null;
            }
            size = 0;
            numberBuckets *= 2;

            for (int i = 0; i < temp.Length; i++)
            {
                Node head = temp[i];

                while (head != null)
                {
                    object key = head.key;
                    object val = head.value;

                    Add(key, val);
                    head = head.next;
                }
            }

            Console.WriteLine("\n***Rehashing Ended***\n");
        }
        #endregion

        #region [ GetValue(object Key) ] Tìm Giá Trị Value Từ Key Đã Cho, Nếu Không Tìm Thấy Trả Về Null
        public object GetValue(object key)
        {
            var bucketind = HashCode(key);
            Node hey2 = buckets[bucketind];
            if (hey2 != null)
            {
                while (hey2.key != key && hey2.next != null)
                {
                    hey2 = hey2.next;
                }
                return hey2.value;
            }
            return null;
        }
        #endregion   

        #region Thuộc Tính [ Keys ] : lấy ra tất cả các Keys có trong Hashtable
        public object[] Keys { get => GetKeys().ToArray(); }
        public IEnumerable<object> GetKeys()
        {
            foreach (Node item in buckets)
            {
                if (item == null)
                    continue;
                yield return item.key;
            }
        }
        #endregion

        #region Thuộc Tính [ Values ] : lấy ra tất cả các Values có trong Hashtable
        public object[] Values { get => GetValues().ToArray(); }
        public IEnumerable<object> GetValues()
        {
            foreach (var item in buckets)
            {
                if (item == null)
                    continue;
                yield return item.value;
            }
        }
        #endregion 

        #region [ Remove() ] Xóa một Key-Value Trong HashTable
        public void Remove(object key)
        {
            var temp = HashCode(key);
            if (buckets[temp] != null)
            {
                Node hey = buckets[temp];
                if (hey.key == key)
                    buckets[temp] = hey.next;
                else
                {
                    while (hey.next != null && hey.next != key && hey.next.next != null)
                    {
                        hey = hey.next;
                    }
                    hey.next = hey.next.next;
                }
                size--;
            }
        }
        #endregion

        public void printHash()
        {
            Node[] temp = buckets;

            Console.WriteLine("HashTable Hiện Tại:");
            for (int i = 0; i < temp.Length; i++)
            {
                Node head = temp[i];

                while (head != null)
                {
                    Console.WriteLine("key = " + head.key + ", val = " + head.value);
                    head = head.next;
                }
            }
            Console.WriteLine();
        }
    }
}