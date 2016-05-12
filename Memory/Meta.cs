using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory
{
    public class memoryitem
    {
        public string type;
        public int size;
        public int starting_add;
        public memoryitem() { }
        public void setsize(int s) { size = s; }
        public int getsize() { return size; }
        public int getstratingadd() { return starting_add; }
        public void setstarting_add(int address) { starting_add = address; }

        public virtual string getname() { return ""; }
    }
    class process : memoryitem
    {
        private string name;
        //private int allocated_space;
        public process(string n, int s) { name = n; size = s; type = "p"; }
        public override string getname() { return name; }

    };
    class blackhole : memoryitem
    {
        private string name;
        //private int allocated_space;
        public blackhole(int add , int s) { name = "system"; size = s; type = "s"; starting_add = add; }
        public override string getname() { return name; }

    };
    class hole : memoryitem
    {
        public hole(int add = 0, int s = 0) { size = s; starting_add = add; type = "h"; }

    };
    class methodology
    {   
        public static List<memoryitem> memory = new List<memoryitem>();

        public static List<memoryitem> allprocess = new List<memoryitem>();
        public static int holes_counter =0;
        public static void blackbox(hole hole)
        {
      
            List<memoryitem> temp = memory.OrderBy(a => a.getstratingadd()).ToList();
            memoryitem last_item;
            int st_add;
            int t_size;
            if (temp.Count > 0)
            {
                last_item = temp.Last();
                t_size = last_item.size;
                st_add = last_item.getstratingadd();
                if ((st_add + t_size < hole.starting_add))
                {
                    blackhole before = new blackhole(st_add + t_size, (hole.starting_add - st_add - t_size));
                    memory.Add(before);
                }
            }

        }  
        public static void concatenate(List<memoryitem> L) // approved
        {
            List<memoryitem> temp = L;
            List<memoryitem> holes = new List<memoryitem>();
            hole h = new hole();
            foreach (var item in temp)
            {
                if (item.type == "h")
                {
                    holes.Add(item);
                }
            }
            holes = holes.OrderBy(add => add.getstratingadd()).ToList();
            for (int i = 0; i < holes.Count - 1; i++)
            {
                int size1 = holes[i].getsize();
                int size2 = holes[i + 1].getsize();
                int add1 = holes[i].getstratingadd();
                int add2 = holes[i + 1].getstratingadd();
                if (add1 + size1 == add2)
                {
                    L.Remove(holes[i]);
                    L.Remove(holes[i + 1]);
                    h.setsize(size1 + size2);
                    h.setstarting_add(add1);
                    L.Add(h);
                }
            }



        }
        public static void inserthole(hole hole)
        {
            // check holes 
            //if(hole.setstarting_add)
            
           
            foreach (var item in memory)
            {
                if (item.type == "s")
                {
                  if((item.starting_add +item.size <= hole.getstratingadd()))
                    {   //case 1 ;
                        //el case de m3naha en el hole el gdeda htkon b3d el item ely ana wa2f feh fa akml itteration 3shan aro7 ll item el next 
                        //3shan hwa da el item el mohem 
                        continue;
                    }
                    else if ((item.starting_add <= hole.getstratingadd()) && (item.starting_add + item.size > hole.getstratingadd()))
                    {   //case 2.1 ;
                        //remove the black hole and add the new blackhole and the new hole
                        // two black hole one before the hole and one after 
                        //but we can use the black hole function to create these blackboxes
                        //so we will delete only and add the hole 

                        int starting_add = item.starting_add;
                        int size_temp = item.size;
                        
                        blackhole before = new blackhole(starting_add, (hole.starting_add - starting_add));
                        blackhole after = new blackhole(hole.starting_add + hole.size, starting_add + size_temp - hole.starting_add - hole.size);
                        memory.Remove(item);
                        memory.Add(hole);
                        memory.Add(before);
                        memory.Add(after);
                        return;
                    }
                    else if ((item.starting_add <= hole.getstratingadd()) && (item.starting_add + item.size < hole.getstratingadd()+hole.size))
                    {
                        //case 2.2;
                        //hna el hole htb2a nosha fe black hole w el nos el tany either over lap with process or another hole ;
                        //we are going tp ignore the hole inthose cases or we can fill the hole till the overlpping point 

                    }
                }
                else if(item.type == "p")
                {
                    if ((item.starting_add + item.size <= hole.getstratingadd()))
                    {   //case 1 ;
                        //el case de m3naha en el hole el gdeda htkon b3d el item ely ana wa2f feh fa akml itteration 3shan aro7 ll item el next 
                        //3shan hwa da el item el mohem 
                        continue;
                    }
                    else if ((item.starting_add <= hole.getstratingadd()) && (item.starting_add + item.size > hole.getstratingadd()))
                    {
                        //send error w break ;
                        break;

                    }else{
                        //send error and break;
                        break;
                    }
                }else
                {
                    if ((item.starting_add + item.size <= hole.getstratingadd()))
                    {   //case 1 ;
                        //el case de m3naha en el hole el gdeda htkon b3d el item ely ana wa2f feh fa akml itteration 3shan aro7 ll item el next 
                        //3shan hwa da el item el mohem 
                        continue;
                    }
                    else if ((item.starting_add <= hole.getstratingadd()) && (item.starting_add + item.size >= hole.getstratingadd()))
                    {
                        //send error w break ;
                        return;

                    }
                    else
                    {
                        //send error and break;
                        return;
                    }
                }
                
            }

            blackbox(hole);
            memory.Add(hole);
            concatenate(memory);

        }
        public static List<memoryitem> FirstFit(process newprocess) //approved
        {
            process nprocess = newprocess;

            foreach (var item in memory)
            {
                if (item.type == "h")
                {
                    if (item.size >= newprocess.size)
                    {
                        //get el starting address w el size  
                        int startaddress = item.getstratingadd();
                        int size = item.getsize();
                        //add new process in pos (starting address) 
                        nprocess.setstarting_add(startaddress);
                        memory.Add(nprocess);
                        //add new hole in pos start address+size of process its size equals ( old hole size - new process size)
                        hole nhole = new hole((startaddress + nprocess.getsize()), (size - nprocess.getsize()));
                        memory.Add(nhole);
                        //delete el hole el adema 
                        memory.Remove(item);
                        allprocess.Add(nprocess);
                        
                        break;
                    }
                }

            }
            concatenate(memory);
            return memory;
        }

        public static List<memoryitem> BestFit(process newprocess)
        {
            process nprocess = newprocess;
            List<memoryitem> temp = memory;
            temp = memory.OrderBy(a => a.getsize()).ToList();

            foreach (var item in temp)
            {

                if (item.type == "h")
                {
                    if (item.size >= newprocess.size)
                    {
                        //get el starting address w el size  
                        int startaddress = item.getstratingadd();
                        int size = item.getsize();
                        //add new process in pos (starting address) 
                        nprocess.setstarting_add(startaddress);
                        memory.Add(nprocess);
                        //add new hole in pos start address+size of process its size equals ( old hole size - new process size)
                        hole nhole = new hole((startaddress + nprocess.getsize()), (size - nprocess.getsize()));
                        memory.Add(nhole);
                        //delete el hole el adema 
                        memory.Remove(item);
                        allprocess.Add(nprocess);
                        break;
                    }

                }

            }

            concatenate(memory);
            return memory;
        }

        public static List<memoryitem> WorstFit(process newprocess) //approved
        {
            process nprocess = newprocess;
            List<memoryitem> temp = memory;
            temp=memory.OrderByDescending(d => d.getsize()).ToList();
            foreach (var item in temp)
            {
                if (item.type == "h")
                {
                    if (item.size >= newprocess.size)
                    {
                        //get el starting address w el size  
                        int startaddress = item.getstratingadd();
                        int size = item.getsize();
                        //add new process in pos (starting address) 
                        nprocess.setstarting_add(startaddress);
                        memory.Add(nprocess);
                        //add new hole in pos start address+size of process its size equals ( old hole size - new process size)
                        hole nhole = new hole((startaddress + nprocess.getsize()), (size - nprocess.getsize()));
                        memory.Add(nhole);
                        //delete el hole el adema 
                        memory.Remove(item);
                        allprocess.Add(nprocess);
                        break;
                    }

                }

            }
            concatenate(memory);
            return memory;
        }
        public static List<memoryitem> deallocate(string processname)
        {
            string name = processname;

            foreach (var item in memory)
            {
                if (item.type == "p" && item.getname() == name)
                {
                    int size = item.getsize();
                    int address = item.getstratingadd();
                    memory.Remove(item);
                    allprocess.Remove(item);
                    hole nhole = new hole((address), (size));
                    memory.Add(nhole);
                    break;
                }

            }
            concatenate(memory);
            return memory;
        }
    };
}
