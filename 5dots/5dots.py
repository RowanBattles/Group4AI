import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv('fivepointsWithDatetime.csv')
df = df.sort_values(by='student_id', ascending=True)

number_of_students_to_process = 30 
average_students = [6776, 1572, 6413, 284, 2147, 5230, 7206, 2998, 1439, 8038, 5886, 7027, 5521, 4828, 8029, 6525, 563, 8232, 9620, 6827]

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
dot_coords = [(0, 0), (2, 0), (0, 2), (2, 2), (1, 1)]

def apply_offset(points, x_offset, y_offset):
    return [(x + x_offset, y + y_offset) for x, y in points]

unique_student_ids = df['student_id'].unique()[:number_of_students_to_process]

for user_id in average_students:
    user_df = df[df['student_id'] == user_id]
    user_df = user_df.sort_values(by='datetime', ascending=True)
    user_df.reset_index(drop=True, inplace=True)

    num_pauses = 0
    x_offset = 0
    y_offset = 0
    max_x_value = 2

    for index, row in user_df.iterrows():
        timestampsm = row['timestampsm']
        line_pattern = row['patternsm'].replace(',', '')
        num_lines = line_pattern.count('1')
        threshold_time = 5000 + (800 * num_lines)

        if timestampsm > threshold_time:
            num_pauses += 1

    plot_height = 3 + num_pauses * 2

    total_width = 0
    for index, row in user_df.iterrows():
        timestampsm = row['timestampsm']
        space_increment = timestampsm / 1000
        total_width += (max_x_value + 1) + space_increment
    
    fig, ax = plt.subplots(figsize=(total_width / 10, plot_height), dpi=300)

    for index, row in user_df.iterrows():
        timestampsm = row['timestampsm']
        space_increment = timestampsm / 1000
        line_pattern = row['patternsm'].replace(',', '')
        num_lines = line_pattern.count('1')
        threshold_time = 5000 + (800 * num_lines)

        if timestampsm > threshold_time:
            x_offset = 0
            y_offset -= (max_x_value + 1) + space_increment

        for i, char in enumerate(line_pattern):
            if char == '1':
                line = i + 1
                if line in lines:
                    points = lines[line]
                    shifted_points = apply_offset(points, x_offset, y_offset)
                    ax.plot([p[0] for p in shifted_points], [p[1] for p in shifted_points], 'k-', linewidth=0.8)

        for dot in dot_coords:
            shifted_dot = apply_offset([dot], x_offset, y_offset)[0]
            ax.plot(shifted_dot[0], shifted_dot[1], 'ko', markersize=1.6)

        x_offset += (max_x_value + 1) + space_increment

    plt.xlim(-1, total_width)
    plt.ylim(y_offset - 10, 3)
    plt.axis('scaled')
    plt.axis('off')
    plt.savefig(f'results/user_{user_id}_figures.png')
    plt.clf()

print("All figures have been processed and saved.")