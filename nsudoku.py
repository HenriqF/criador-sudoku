import copy
import random

class Sudoku:

    def solve_count_solutions(self, board, solution, random_numbers = 0):
        """
            resolve um tabuleiro sem contradicoes, poe em solution e mostra se ha multiplas solucoes:
            0 -> uma solucao / nenhuma.
            1 -> 2+ solucoes.
        """
        #tabuleiroinfo
        rows=[set()for _ in range(self.size)]
        columns=[set()for _ in range(self.size)]
        blocks=[set()for _ in range(self.size)]
        empty=[]

        for r in range(self.size):
            for c in range(self.size):
                if board[r][c] == '_':
                    empty.append((r,c))
                else:
                    ch=board[r][c]
                    rows[r].add(ch)
                    columns[c].add(ch)
                    blocks[(r//self.blockheight)*self.blockheight+(c//self.blocklength)].add(ch)

        #resolver
        solutions = 0
        first_solution = []
        def solve_from(index):
            if index == len(empty):
                nonlocal solutions
                solutions += 1

                if solutions == 1:
                    nonlocal first_solution
                    first_solution = copy.deepcopy(solution)

                if solutions >= 2:
                    return 1        

                return 0
            
            r = empty[index][0]
            c = empty[index][1]
            b = (r//self.blockheight)*self.blockheight+(c//self.blocklength)


            n = self.numeros
            if (random_numbers):
                random.shuffle(n) 

            for ch in n:
                if ((ch not in rows[r]) and (ch not in columns[c]) and (ch not in blocks[b])):
                    solution[r][c]=ch
                    rows[r].add(ch)
                    columns[c].add(ch)
                    blocks[b].add(ch)

                    if solve_from(index+1):
                        return 1
                    
                    solution[r][c] = '_'
                    rows[r].remove(ch)
                    columns[c].remove(ch)
                    blocks[b].remove(ch)

            return 0
        
        result = solve_from(0)
        solution[:] = first_solution
        return result

    def new_valid_boards(self, board, solution):
        #criacao da porraloca
        new = [['_' for _ in range(self.size)] for _ in range(self.size)]
        master = copy.deepcopy(new)
        self.solve_count_solutions(new, master, random_numbers=1)

        solution[:] = copy.deepcopy(master)

        #remocao de celulas para poder obter o resultado que resulta-se do nao-resultado (nao processado)
        count = 0

        size_sqr = self.size*self.size
        to_remove = random.randint((int)(size_sqr*0.55), (int)(size_sqr*0.60))


        not_removed = []
        for r in range(0, self.size):
            for c in range(0, self.size):
                not_removed.append((r,c))
        random.shuffle(not_removed)
        
        while count < to_remove:
            while 1:
                if len(not_removed) == 0:
                    break

                r,c = not_removed.pop()

                newmaster = copy.deepcopy(master)
                newmaster[r][c] = '_'

                if (self.solve_count_solutions(newmaster, newmaster) == 0):
                    master[r][c] = '_'
                    break

            count += 1


        board[:] = master

    def show_boards(self):
        print("="*40)
        for i in range(self.size):
            print(self.board[i], ",", sep ="")
        print("\n")
        for i in range(self.size):
            print(self.solution[i], ",", sep ="")
        print("="*40)

    def __init__(self):
        self.size = 6
        self.blockheight = 2
        self.blocklength = 3
        self.numeros = [str(i+1) for i in range(self.size)]


        self.board = []
        self.solution = []
        self.new_valid_boards(self.board, self.solution)


s = Sudoku().show_boards()