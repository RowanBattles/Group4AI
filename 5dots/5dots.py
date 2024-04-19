import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv('restructured_fivepointstest.csv')

number_of_students_to_process = 10 

lines = {
    1: [(0, 2), (2, 2)],
    2: [(2, 0), (2, 2)],
    3: [(0, 0), (2, 0)],
    4: [(0, 0), (0, 2)],
    5: [(0, 2), (1, 1)],
    6: [(2, 2), (1, 1)],
    7: [(2, 0), (1, 1)],
    8: [(0, 0), (1, 1)],
}
dot_coords = [(0,0), (2,0), (0,2), (2,2), (1,1)]

def apply_offset(points, x_offset, y_offset):
    return [(x + x_offset, y + y_offset) for x, y in points]

unique_student_ids = df['student_id'].unique()[:number_of_students_to_process]

for user_id in unique_student_ids:
    user_df = df[df['student_id'] == user_id]

    fig, ax = plt.subplots()
    x_offset = 0
    y_offset = 0
    max_x_value = 2
    picture_count = 0
    dot_size = 2

    for index, row in user_df.iterrows():
        line_pattern = row['patternsm'].replace(',', '')
        
        for i, char in enumerate(line_pattern):
            if char == '1':
                line = i + 1
                if line in lines:
                    points = lines[line]
                    shifted_points = apply_offset(points, x_offset, y_offset)
                    ax.plot([p[0] for p in shifted_points], [p[1] for p in shifted_points], 'k-')

        for dot in dot_coords:
            shifted_dot = apply_offset([dot], x_offset, y_offset)[0]
            ax.plot(shifted_dot[0], shifted_dot[1], 'ko', markersize=dot_size)

        picture_count += 1
        if picture_count % 5 == 0:
            x_offset = 0
            y_offset -= (max_x_value + 1)
        else:
            x_offset += (max_x_value + 1)

    plt.xlim(-1, (max_x_value + 1) * 5)
    plt.ylim(y_offset - max_x_value - 1, 3)

    plt.axis('scaled')
    plt.axis('off')

    plt.savefig(f'results/user_{user_id}_figures.png')
    plt.clf()

print("All user figures have been processed and saved.")
