import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv('5dots.csv', delimiter=';')

lines = {
    1: [(0, 2), (2, 2)],  # Top horizontal
    2: [(2, 0), (2, 2)],  # Right vertical
    3: [(0, 0), (2, 0)],  # Bottom horizontal
    4: [(0, 0), (0, 2)],  # Left vertical
    5: [(0, 2), (1, 1)],  # Top-left to middle
    6: [(2, 2), (1, 1)],  # Top-right to middle
    7: [(0, 0), (1, 1)],  # Bottom-left to middle
    8: [(2, 0), (1, 1)],  # Bottom-right to middle
}

# Dots
dot_coords = [(0,0), (1,1), (1,2), (2,1), (2,2)]

def apply_offset(points, offset):
    """Apply an offset to the x-coordinates of the points."""
    return [(x + offset, y) for x, y in points]

for user_id in df['user_id'].unique():
    user_df = df[df['user_id'] == user_id]

    fig, ax = plt.subplots()

    offset = 0

    max_x_value = 2

    # Go through each picture number for the selected user
    for picture_number in sorted(user_df['picture_number'].unique()):
        picture_df = user_df[user_df['picture_number'] == picture_number]
        for index, row in picture_df.iterrows():
            clicked_lines = [int(x.strip()) for x in row['lines_selected'].split(',')]
            # Draw each line that was clicked
            for line in clicked_lines:
                if line in lines:
                    points = lines[line]
                    shifted_points = apply_offset(points, offset)
                    ax.plot([p[0] for p in shifted_points], [p[1] for p in shifted_points], 'k-')

        # Add dots with the applied offset
        for dot in dot_coords:
            shifted_dot = apply_offset([dot], offset)[0]
            ax.plot(shifted_dot[0], shifted_dot[1], 'ko')

        # Update the offset for the next figure
        offset += max_x_value + 1

    plt.axis('scaled')
    plt.axis('off')

    plt.savefig(f'results/user_{user_id}_figures.png')

    plt.clf()

print("All user figures have been processed and saved.")
